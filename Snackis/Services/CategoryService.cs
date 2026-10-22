using Snackis.Models;
using Snackis.Repositories;

namespace Snackis.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Category>> GetAllAsync() => _repository.GetAllAsync();

    public Task<Category?> GetAsync(int id) => _repository.GetByIdAsync(id);

    // Every category except the one being edited (it can't be its own parent)
    public async Task<List<Category>> GetPossibleParentsAsync(int? excludeId)
    {
        var all = await _repository.GetAllAsync();
        return all.Where(c => c.Id != excludeId).ToList();
    }

    public Task CreateAsync(Category category) => _repository.AddAsync(category);

    // Returns null on success, or an error message
    public async Task<string?> UpdateAsync(int id, string name, int? parentCategoryId)
    {
        if (parentCategoryId == id)
            return "A category can't be its own parent.";

        var category = await _repository.GetByIdAsync(id);
        if (category == null)
            return "The category was not found.";

        category.Name = name;
        category.ParentCategoryId = parentCategoryId;
        await _repository.SaveChangesAsync();
        return null;
    }

    // Returns null on success, or an error message
    public async Task<string?> DeleteAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null) return null;

        if (await _repository.HasChildrenAsync(id) || await _repository.HasPostsAsync(id))
            return "This category still has subcategories or posts, so it can't be deleted.";

        await _repository.DeleteAsync(category);
        return null;
    }
}