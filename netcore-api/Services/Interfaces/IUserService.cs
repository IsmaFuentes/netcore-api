using System.Linq.Expressions;

namespace netcore_api.Services.Interfaces
{
  public interface IUserService
  {
    public Task<Contracts.DTO.PaginationResultDto> GetUsersAsync(Expression<Func<Data.Entities.User, bool>>? expression = null, int page = 1, int pageSize = 100);
    public Task<Contracts.DTO.UserDto?> GetUserAsync(int id);
    public Task<Contracts.DTO.UserDto> CreateUserAsync(Contracts.DTO.UserRegistrationDto dto);
    public Task<Contracts.DTO.UserDto> UpdateUserAsync(Contracts.DTO.UserDto dto);
    public Task<Contracts.DTO.UserDto> DeleteUserAsync(int id);
  }
}
