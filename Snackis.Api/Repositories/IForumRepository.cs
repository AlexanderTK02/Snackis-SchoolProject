using Snackis.Api.Models;

namespace Snackis.Api.Repositories;

public interface IForumRepository
{
    Task<List<Category>> GetCategoriesAsync();
    Task<Category?> GetCategoryAsync(int id);
    Task<List<Post>> GetPostsByCategoryAsync(int categoryId);
}