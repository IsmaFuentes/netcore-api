using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace netcore_api.Services
{
  public class JwtTokenService : Interfaces.IJwtTokenService
  {
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public string GenerateToken(Data.Entities.User user)
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
          new Claim(ClaimTypes.Role, user.Role.ToString())
        ]),
      });

      return handler.WriteToken(token);
    }
  }
}
