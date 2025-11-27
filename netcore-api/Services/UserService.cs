using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace netcore_api.Services
{
  public interface IUserService
  {
    public Task<List<Contracts.DTO.UserDto>> GetUsersAsync(Expression<Func<Data.Entities.User, bool>>? expression = null, int page = 1, int pageSize = 100);
    public Task<Contracts.DTO.UserDto?> GetUserAsync(int id);
    public Task<Contracts.DTO.UserDto> CreateUserAsync(Contracts.DTO.UserRegistrationDto dto);
    public Task<Contracts.DTO.UserDto> UpdateUserAsync(Contracts.DTO.UserDto dto);
    public Task<Contracts.DTO.UserDto> DeleteUserAsync(int id);
  }

  public class UserService : IUserService
  {
    private readonly Data.Context _context;
    private readonly PasswordHasher<Data.Entities.User> _hasher;
    private readonly ILogger<UserService> _logger;

    public UserService(
      Data.Context context, 
      PasswordHasher<Data.Entities.User> hasher, 
      ILogger<UserService> logger) 
    {
      _context = context;
      _hasher = hasher;
      _logger = logger;
    }  

    public async Task<List<Contracts.DTO.UserDto>> GetUsersAsync(
      Expression<Func<Data.Entities.User, bool>>? expression = null, 
      int page = 1, 
      int pageSize = 100)
    {
      var query = _context.Users.AsNoTracking();

      if(expression is not null)
      {
        query = query.Where(expression);
      }

      query = query.OrderBy(e => e.Id).Skip((page - 1) * pageSize).Take(pageSize);

      var result = await query
        .Select(e => new Contracts.DTO.UserDto()
        {
          Id = e.Id,
          UserName = e.UserName,
          RegistrationDate = e.RegistrationDate,
          IsActive = e.IsActive,
          Role = (int)e.Role
        })
        .ToListAsync();

      return result;
    }

    public async Task<Contracts.DTO.UserDto?> GetUserAsync(int id)
    {
      var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == id);

      if(user is not null)
      {
        return new Contracts.DTO.UserDto()
        {
          Id = user.Id,
          UserName = user.UserName,
          RegistrationDate = user.RegistrationDate,
          IsActive = user.IsActive,
          Role = (int)user.Role
        };
      }

      _logger.LogWarning($"User with id {id} not found");

      return default;
    }

    public async Task<Contracts.DTO.UserDto> CreateUserAsync(Contracts.DTO.UserRegistrationDto dto)
    {
      if (await _context.Users.AnyAsync(e => e.UserName == dto.UserName))
        throw new Exceptions.UserAlreadyExistsException("your username is already in use.");

      var user = new Data.Entities.User();

      user.UserName = dto.UserName;
      user.Role = (Data.Entities.UserRole)dto.Role;
      user.IsActive = true;
      user.Password = _hasher.HashPassword(user, dto.Password);

      await _context.Users.AddAsync(user);
      await _context.SaveChangesAsync();

      return new Contracts.DTO.UserDto()
      {
        Id = user.Id,
        UserName = user.UserName,
        RegistrationDate = user.RegistrationDate,
        IsActive = user.IsActive,
        Role = (int)user.Role
      };
    }

    public async Task<Contracts.DTO.UserDto> UpdateUserAsync(Contracts.DTO.UserDto dto)
    {
      var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == dto.Id);

      if (user is null)
      {
        _logger.LogWarning($"Unable to update user with id {dto.Id} - User not found");
        throw new Exceptions.NotFoundException("user not found");
      }

      user.Role = (Data.Entities.UserRole)dto.Role;
      user.IsActive = dto.IsActive;

      await _context.SaveChangesAsync();

      return new Contracts.DTO.UserDto()
      {
        Id = user.Id,
        UserName = user.UserName,
        RegistrationDate = user.RegistrationDate,
        IsActive = user.IsActive,
        Role = (int)user.Role
      };
    }

    public async Task<Contracts.DTO.UserDto> DeleteUserAsync(int id)
    {
      var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == id);

      if (user is null)
      {
        _logger.LogWarning($"Unable to delete user with id {id} - User not found");
        throw new Exceptions.NotFoundException("user not found");
      }

      user.IsDeleted = true;
      user.DeletedAt = DateTime.Now;

      await _context.SaveChangesAsync();

      return new Contracts.DTO.UserDto()
      {
        Id = user.Id,
        UserName = user.UserName,
        RegistrationDate = user.RegistrationDate,
        IsActive = user.IsActive,
        Role = (int)user.Role,
        IsDeleted = user.IsDeleted,
        DeletedAt = user.DeletedAt,
      };
    }
  }
}
