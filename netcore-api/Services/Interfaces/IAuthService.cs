using netcore_api.Contracts.DTO;

namespace netcore_api.Services
{
  public interface IAuthService
  {
    public Task<AuthResponseDto?> Login(AuthRequestDto authRequest);
    public Task<AuthResponseDto> RefreshToken(int userId);
  }
}
