using Snackis.Models;
using Snackis.Repositories;

namespace Snackis.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _repository;

    public PostService(IPostRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Post>> GetAllAsync() => _repository.GetAllAsync();

    public Task<Post?> GetAsync(int id) => _repository.GetByIdAsync(id);

    // Only the author may edit
    public bool CanEdit(Post post, string userId) => post.UserId == userId;

    // The author or an admin may delete
    public bool CanDelete(Post post, string userId, bool isAdmin) => isAdmin || post.UserId == userId;

    public async Task CreateAsync(Post post, string userId)
    {
        post.UserId = userId;
        post.CreatedAt = DateTime.Now;
        await _repository.AddAsync(post);
    }

    public async Task<OperationResult> UpdateAsync(int id, string title, string content, int categoryId, string userId)
    {
        var post = await _repository.GetByIdAsync(id);
        if (post == null) return OperationResult.NotFound;
        if (!CanEdit(post, userId)) return OperationResult.Forbidden;

        post.Title = title;
        post.Content = content;
        post.CategoryId = categoryId;
        await _repository.SaveChangesAsync();
        return OperationResult.Success;
    }

    public async Task<OperationResult> DeleteAsync(int id, string userId, bool isAdmin)
    {
        var post = await _repository.GetByIdAsync(id);
        if (post == null) return OperationResult.NotFound;
        if (!CanDelete(post, userId, isAdmin)) return OperationResult.Forbidden;

        await _repository.DeleteAsync(post);
        return OperationResult.Success;
    }
}