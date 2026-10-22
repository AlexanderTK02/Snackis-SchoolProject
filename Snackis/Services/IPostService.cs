using Snackis.Models;

namespace Snackis.Services;

public interface IPostService
{
    Task<List<Post>> GetAllAsync();
    Task<Post?> GetAsync(int id);
    bool CanEdit(Post post, string userId);
    bool CanDelete(Post post, string userId, bool isAdmin);
    Task CreateAsync(Post post, string userId);
    Task<OperationResult> UpdateAsync(int id, string title, string content, int categoryId, string userId);
    Task<OperationResult> DeleteAsync(int id, string userId, bool isAdmin);
}