using Snackis.Models;

namespace Snackis.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<bool> HasChildrenAsync(int id);
    Task<bool> HasPostsAsync(int id);
    Task AddAsync(Category category);
    Task DeleteAsync(Category category);
    Task SaveChangesAsync();
}