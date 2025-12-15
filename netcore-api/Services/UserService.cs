using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using netcore_api.Mapping;

namespace netcore_api.Services
{
  public class UserService : Interfaces.IUserService
  {
    private readonly Data.Context _context;
    private readonly IPasswordHasher<Data.Entities.User> _hasher;
    private readonly ILogger<UserService> _logger;

    public UserService(
      Data.Context context, 
      IPasswordHasher<Data.Entities.User> hasher, 
      ILogger<UserService> logger) 
    {
      _context = context;
      _hasher = hasher;
      _logger = logger;
    }  

    public async Task<Contracts.DTO.PaginationResultDto> GetUsersAsync(
      System.Linq.Expressions.Expression<Func<Data.Entities.User, bool>>? expression = null, 
      int page = 1, 
      int pageSize = 100)
    {
      var query = _context.Users.AsNoTracking().Where(e => e.IsActive && !e.IsDeleted);
      int count = await query.Where(e => e.IsActive && !e.IsDeleted).CountAsync();

      if(expression is not null)
      {
        query = query.Where(expression);
      }

      query = query.OrderBy(e => e.Id).Skip((page - 1) * pageSize).Take(pageSize);

      var result = await query
        .Select(e => e.MapToUserDto())
        .ToListAsync();

      return new Contracts.DTO.PaginationResultDto() 
      {
        Page = page,
        Count = count,
        Results = result
      };
    }

    public async Task<Contracts.DTO.UserDto?> GetUserAsync(int id)
    {
      var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

      if(user is not null)
      {
        return user.MapToUserDto();
      }

      _logger.LogWarning($"User with id {id} not found");

      return default;
    }

    public async Task<Contracts.DTO.UserDto> CreateUserAsync(Contracts.DTO.UserRegistrationDto dto)
    {
      if (await _context.Users.AnyAsync(e => e.UserName == dto.UserName))
        throw new Exceptions.UserAlreadyExistsException("Username is already in use.");

      var user = new Data.Entities.User();

      user.UserName = dto.UserName;
      user.Role = (Data.Entities.UserRole)dto.Role;
      user.IsActive = true;
      user.Password = _hasher.HashPassword(user, dto.Password);

      await _context.Users.AddAsync(user);
      await _context.SaveChangesAsync();

      return user.MapToUserDto();
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

      return user.MapToUserDto();
    }

    public async Task<Contracts.DTO.UserDto> DeleteUserAsync(int id)
    {
      var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == id);

      if (user is null)
      {
        _logger.LogWarning($"Unable to delete user with id {id} - User not found");
        throw new Exceptions.NotFoundException("user not found");
      }

      user.IsActive = false;
      user.IsDeleted = true;
      user.DeletedAt = DateTime.Now;

      await _context.SaveChangesAsync();

      return user.MapToUserDto();
    }
  }
}
