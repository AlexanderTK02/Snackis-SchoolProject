using Microsoft.EntityFrameworkCore;
using Snackis.Api.Data;
using Snackis.Api.Models;

namespace Snackis.Api.Repositories;

public class ForumRepository : IForumRepository
{
    private readonly ForumDbContext _context;

    public ForumRepository(ForumDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetCategoryAsync(int id)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    // Posts in the category itself, plus posts in its direct subcategories
    public async Task<List<Post>> GetPostsByCategoryAsync(int categoryId)
    {
        return await _context.Posts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.User)
            .Where(p => p.CategoryId == categoryId || p.Category!.ParentCategoryId == categoryId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }
}