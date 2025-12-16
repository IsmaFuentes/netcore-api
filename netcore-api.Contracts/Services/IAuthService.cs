namespace netcore_api.Contracts.Services
{
  public interface IAuthService
  {
    public Task<DTO.AuthResponseDto?> Login(DTO.AuthRequestDto authRequest);
    public Task<DTO.AuthResponseDto> RefreshToken(int userId);
  }
}
