using Snackis.ApiModels;

namespace Snackis.Services;

public interface IForumApiClient
{
    Task<List<ApiCategory>?> GetCategoriesAsync();
    Task<List<ApiPost>?> GetPostsAsync(int categoryId);
    Task<List<ApiPost>?> GetAllPostsAsync();
}
