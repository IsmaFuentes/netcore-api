using Microsoft.EntityFrameworkCore;
using netcore_api.Data.Entities;

namespace netcore_api.Data.Repositories
{
  public interface IUserRepository
  {
    Task<List<User>> GetAsync(int page = 1, int pageSize = 100);
    Task<int> CountAsync();
    Task<User?> GetAsync(int id);
    Task<bool> ExistsAsync(string userName);
    Task AddAsync(User entity);
    Task UpdateAsync(User entity);
    Task DeleteAsync(User entity);
  }

  public class UserRepository : IUserRepository
  {
    private readonly Context _context;

    public UserRepository(Context context)
    {
      _context = context;
    }

    public async Task<int> CountAsync()
    {
      return await _context.Users.AsNoTracking().Where(e => e.IsActive && !e.IsDeleted).CountAsync();
    }

    public async Task<bool> ExistsAsync(string userName)
    {
      return await _context.Users.AsNoTracking().AnyAsync(e => e.UserName == userName);
    }

    public async Task<List<User>> GetAsync(int page = 1, int pageSize = 100)
    {
      var query = _context.Users.AsNoTracking()
        .Where(e => e.IsActive && !e.IsDeleted).Skip((page - 1) * pageSize).Take(pageSize);

      return await query.ToListAsync();
    }

    public virtual async Task<User?> GetAsync(int id)
    {
      return await _context.Users.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
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
