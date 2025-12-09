namespace netcore_api.Services.Interfaces
{
  public interface IAuthService
  {
    public Task<Contracts.DTO.AuthResponseDto?> Login(Contracts.DTO.AuthRequestDto authRequest);
    public Task<Contracts.DTO.AuthResponseDto> RefreshToken(int userId);
  }
}
