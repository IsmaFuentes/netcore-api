using Microsoft.AspNetCore.Identity;
using netcore_api.Data.Repositories;
using netcore_api.Contracts.Services;
using netcore_api.Mapping;

namespace netcore_api.Services
{
  public class UserService : IUserService
  {
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher<Data.Entities.User> _hasher;
    private readonly ILogger<UserService> _logger;

    public UserService(
      IUserRepository userRepository, 
      IPasswordHasher<Data.Entities.User> hasher, 
      ILogger<UserService> logger) 
    {
      _repository = userRepository;
      _hasher = hasher;
      _logger = logger;
    }  

    public async Task<Contracts.DTO.PaginationResultDto> GetUsersAsync(int page = 1, int pageSize = 100)
    {
      var result = await _repository.GetAsync(page, pageSize);
      int count = await _repository.CountAsync();

      return new Contracts.DTO.PaginationResultDto() 
      {
        Page = page,
        Count = count,
        Results = result.Select(e => e.MapToUserDto())
      };
    }

    public async Task<Contracts.DTO.UserDto?> GetUserAsync(int id)
    {
      var user = await _repository.GetAsync(id); 

      if(user is not null)
      {
        return user.MapToUserDto();
      }

      _logger.LogWarning($"User with id {id} not found");

      return default;
    }

    public async Task<Contracts.DTO.UserDto> CreateUserAsync(Contracts.DTO.UserRegistrationDto dto)
    {
      if (await _repository.ExistsAsync(dto.UserName))
        throw new Exceptions.UserAlreadyExistsException("Username is already in use.");

      var user = new Data.Entities.User();

      user.UserName = dto.UserName;
      user.Role = (Data.Entities.UserRole)dto.Role;
      user.IsActive = true;
      user.Password = _hasher.HashPassword(user, dto.Password);

      await _repository.AddAsync(user);

      return user.MapToUserDto();
    }

    public async Task<Contracts.DTO.UserDto> UpdateUserAsync(Contracts.DTO.UserDto dto)
    {
      var user = await _repository.GetAsync(dto.Id);

      if (user is null)
      {
        _logger.LogWarning($"Unable to update user with id {dto.Id} - User not found");
        throw new Exceptions.NotFoundException("user not found");
      }

      user.Role = (Data.Entities.UserRole)dto.Role;
      user.IsActive = dto.IsActive;

      await _repository.UpdateAsync(user);

      return user.MapToUserDto();
    }

    public async Task<Contracts.DTO.UserDto> DeleteUserAsync(int id)
    {
      var user = await _repository.GetAsync(id);

      if (user is null)
      {
        _logger.LogWarning($"Unable to delete user with id {id} - User not found");
        throw new Exceptions.NotFoundException("user not found");
      }

      await _repository.DeleteAsync(user);

      return user.MapToUserDto();
    }
  }
}
