using netcore_api.Data.Entities;

namespace netcore_api.Data.Repositories
{
  public interface IUserRepository
  {
    Task<(List<User>, int userCount)> GetAsync(int page = 1, int pageSize = 100);
    Task<User?> GetAsync(int id);
    Task<User?> GetAsync(string userName);
    Task<bool> ExistsAsync(string userName);
    Task AddAsync(User entity);
    Task UpdateAsync(User entity);
    Task DeleteAsync(User entity);
  }
}
