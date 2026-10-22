using Snackis.Api.Dtos;

namespace Snackis.Api.Services;

public interface IForumService
{
    Task<List<CategoryDto>> GetCategoriesAsync();
    Task<List<PostDto>?> GetPostsForCategoryAsync(int categoryId);
}