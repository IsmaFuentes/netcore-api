using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace netcore_api.Services
{
  public interface IAuthService
  {
    public Task<Contracts.DTO.AuthResponseDto?> Login(Contracts.DTO.AuthRequestDto authRequest);
  }

  public class AuthService : IAuthService
  {
    private readonly Data.Context _ctx;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<Data.Entities.User> _hasher;

    public AuthService(IConfiguration configuration, Data.Context context, PasswordHasher<Data.Entities.User> hasher)
    {
      _ctx = context;
      _configuration = configuration;
      _hasher = hasher;
    }

    public async Task<Contracts.DTO.AuthResponseDto?> Login(Contracts.DTO.AuthRequestDto request)
    {
      var user = await _ctx.Users.FirstOrDefaultAsync(e => e.UserName == request.UserName);

      if (user is null)
        throw new Exceptions.NotFoundException("user not found");

      if(user.IsActive)
      {
        if (_hasher.VerifyHashedPassword(user, user.Password, request.Password) == PasswordVerificationResult.Success)
        {
          return new Contracts.DTO.AuthResponseDto { Token = GenerateJwt(user) };
        }
      }

      return default;
    }

    private string GenerateJwt(Data.Entities.User user)
    {
      var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
      var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

      var token = handler.CreateToken(new SecurityTokenDescriptor()
      {
        Issuer = _configuration["Jwt:Issuer"],
        Audience = _configuration["Jwt:Audience"],
        Expires = DateTime.UtcNow.AddDays(1), // Añadir en json de configuración
        SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
        Subject = new ClaimsIdentity(
        [
          new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
          new Claim(ClaimTypes.Name, user.UserName),
          new Claim(ClaimTypes.Role, user.Role.ToString()) // [Authorize(Roles = "Admin")]
        ]),
      });

      return handler.WriteToken(token);
    }
  }
}
