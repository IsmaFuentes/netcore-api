using Microsoft.EntityFrameworkCore;
using netcore_api.Data.Entities;

namespace netcore_api.Data.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly Context _context;

    public UserRepository(Context context)
    {
      _context = context;
    }

    public async Task<bool> ExistsAsync(string userName)
    {
      return await _context.Users.AsNoTracking().AnyAsync(e => e.UserName == userName);
    }

    public async Task<(List<User>, int userCount)> GetAsync(int page = 1, int pageSize = 100)
    {
      if(pageSize <= 0)
        throw new ArgumentOutOfRangeException($"PageSize must be >= 1");

      if (page <= 0)
        throw new ArgumentOutOfRangeException($"Page must be >= 1");

      var query = _context.Users.AsNoTracking().Where(e => e.IsActive && !e.IsDeleted);

      int count = await query.CountAsync();
      var users = await query
        .OrderBy(user => user.Id)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

      return (users, count);
    }

    public virtual async Task<User?> GetAsync(int id)
    {
      return await _context.Users.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
    }

    public virtual async Task<User?> GetAsync(string userName)
    {
      return await _context.Users.FirstOrDefaultAsync(e => e.UserName == userName && !e.IsDeleted);
    }

    public async Task AddAsync(User entity)
    {
      await _context.Users.AddAsync(entity);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User entity)
    {
      _context.Users.Update(entity);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User entity)
    {
      entity.IsActive = false;
      entity.IsDeleted = true;
      entity.DeletedAt = DateTime.Now;
      await _context.SaveChangesAsync();
    }
  }
}
