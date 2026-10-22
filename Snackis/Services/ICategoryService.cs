using Snackis.Models;

namespace Snackis.Services;

public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetAsync(int id);
    Task<List<Category>> GetPossibleParentsAsync(int? excludeId);
    Task CreateAsync(Category category);
    Task<string?> UpdateAsync(int id, string name, int? parentCategoryId);
    Task<string?> DeleteAsync(int id);
}