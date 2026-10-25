using Snackis.Api.Dtos;
using Snackis.Api.Repositories;

namespace Snackis.Api.Services;

public class ForumService : IForumService
{
    private readonly IForumRepository _repository;

    public ForumService(IForumRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await _repository.GetCategoriesAsync();
        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            ParentCategoryId = c.ParentCategoryId
        }).ToList();
    }

    // Returns null if the category doesn't exist
    public async Task<List<PostDto>?> GetPostsForCategoryAsync(int categoryId)
    {
        var category = await _repository.GetCategoryAsync(categoryId);
        if (category == null) return null;

        var posts = await _repository.GetPostsByCategoryAsync(categoryId);
        return posts.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            Author = p.User?.UserName ?? "Unknown",
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name ?? string.Empty
        }).ToList();
    }

    public async Task<List<PostDto>> GetAllPostsAsync()
    {
        var posts = await _repository.GetAllPostsAsync();
        return posts.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            Author = p.User?.UserName ?? "Unknown",
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name ?? string.Empty
        }).ToList();
    }
}