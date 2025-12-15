using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace netcore_api
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      ConfigureServices(builder);

      var app = builder.Build();

      if (app.Environment.IsDevelopment())
      {
        app.MapOpenApi();
#if DEBUG
        // Crear la base de datos 
        InitializeDb(app.Services);
#endif
      }

      app.UseMiddleware<Middleware.ErrorHandlingMiddleware>();
      app.UseHttpsRedirection();
      app.UseAuthorization();
      app.UseCors("MyCorsPolicy");
      app.MapControllers();

      app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
      builder.Services.AddLogging();
      builder.Services.AddControllers(options =>
      {
        options.Filters.Add<Controllers.Filters.ValidationFilter>();
      });

      string? connString = builder.Configuration.GetConnectionString("DefaultConnection");

      // DB context
      builder.Services.AddDbContext<Data.Context>(options => 
        options.UseSqlServer(connString, o => o.UseCompatibilityLevel(Data.Context.GetSqlCompatLevel(connString))));

      // Repositories
      builder.Services.AddScoped<Data.Repositories.IUserRepository, Data.Repositories.UserRepository>();

      // Services
      builder.Services.AddScoped<IPasswordHasher<Data.Entities.User>, PasswordHasher<Data.Entities.User>>();
      builder.Services.AddScoped<Services.Interfaces.IJwtTokenService, Services.JwtTokenService>();
      builder.Services.AddScoped<Services.Interfaces.IAuthService, Services.AuthService>();
      builder.Services.AddScoped<Services.Interfaces.IUserService, Services.UserService>();

      // Esquema de autenticación Jwt
      builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = builder.Configuration["Jwt:Issuer"],
          ValidAudience = builder.Configuration["Jwt:Audience"],
          ClockSkew = TimeSpan.Zero,
          RoleClaimType = ClaimTypes.Role,
          IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };

        // Error handler de autenticación
        options.Events = new JwtBearerEvents
        {
          OnChallenge = async context =>
          {
            context.HandleResponse();
            throw new Exceptions.InvalidTokenException("Invalid token");
          }, 
          
          OnForbidden = async context =>
          {
            throw new Exceptions.UnauthorizedException("You do not have access to this resource");
          }
        };
      });

      builder.Services.AddCors(options => 
        options.AddPolicy("MyCorsPolicy", builder =>
        {
          builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        }));

      // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
      builder.Services.AddOpenApi();
    }

#if DEBUG
    private static void InitializeDb(IServiceProvider serviceProvider)
    {
      using (var scope = serviceProvider.CreateScope())
      {
        var context = scope.ServiceProvider.GetRequiredService<Data.Context>();

        context.Database.EnsureCreated();

        // crear usuarios por defecto
        if (!context.Users.Any())
        {
          var hasher = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.PasswordHasher<Data.Entities.User>>();

          var admin = new Data.Entities.User
          {
            UserName = "Admin",
            IsActive = true,
            Role = Data.Entities.UserRole.Admin
          };

          admin.Password = hasher.HashPassword(admin, "Abc12345!");

          var moderator = new Data.Entities.User
          {
            UserName = "Moderator",
            IsActive = true,
            Role = Data.Entities.UserRole.Moderator
          };

          moderator.Password = hasher.HashPassword(moderator, "Abc12345!");

          var user = new Data.Entities.User
          {
            UserName = "User",
            IsActive = true,
            Role = Data.Entities.UserRole.User
          };

          user.Password = hasher.HashPassword(user, "Abc12345!");

          context.Users.AddRange(admin, moderator, user);
          context.SaveChanges();
        }
      }
    }
#endif
  }
}
