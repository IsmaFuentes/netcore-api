using netcore_api.Contracts.DTO;

namespace netcore_api.Services
{
  public interface IUserService
  {
    public Task<PaginationResultDto> GetUsersAsync(int page = 1, int pageSize = 100);
    public Task<UserDto?> GetUserAsync(int id);
    public Task<UserDto> CreateUserAsync(UserRegistrationDto dto);
    public Task<UserDto> UpdateUserAsync(UserDto dto);
    public Task<UserDto> DeleteUserAsync(int id);
  }
}
