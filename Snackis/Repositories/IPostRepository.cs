using Snackis.Models;

namespace Snackis.Repositories;

public interface IPostRepository
{
    Task<List<Post>> GetAllAsync();
    Task<Post?> GetByIdAsync(int id);
    Task AddAsync(Post post);
    Task DeleteAsync(Post post);
    Task SaveChangesAsync();
}