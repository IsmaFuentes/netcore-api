namespace netcore_api.Services.Jwt
{
  public interface IJwtTokenService
  {
    public string GenerateToken(Data.Entities.User user);
  }
}
