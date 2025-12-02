namespace netcore_api.Services.Interfaces
{
  public interface IJwtTokenService
  {
    public string GenerateToken(Data.Entities.User user);
  }
}
