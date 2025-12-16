namespace netcore_api.Contracts.Services
{
  public interface IUserService
  {
    public Task<DTO.PaginationResultDto> GetUsersAsync(int page = 1, int pageSize = 100);
    public Task<DTO.UserDto?> GetUserAsync(int id);
    public Task<DTO.UserDto> CreateUserAsync(DTO.UserRegistrationDto dto);
    public Task<DTO.UserDto> UpdateUserAsync(DTO.UserDto dto);
    public Task<DTO.UserDto> DeleteUserAsync(int id);
  }
}
