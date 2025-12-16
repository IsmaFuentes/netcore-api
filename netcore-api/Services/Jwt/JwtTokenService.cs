using Microsoft.IdentityModel.Tokens;

namespace netcore_api.Services.Jwt
{
  public class JwtTokenService : IJwtTokenService
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
        Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpirationMinutes"])),
        SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
        Subject = new System.Security.Claims.ClaimsIdentity(
        [
          new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
          new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserName),
          new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, user.Role.ToString())
        ]),
      });

      return handler.WriteToken(token);
    }
  }
}
