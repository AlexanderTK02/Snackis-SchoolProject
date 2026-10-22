using Snackis.Models;

namespace Snackis.Repositories;

public interface IReportRepository
{
    Task<List<Report>> GetAllAsync();
    Task<Report?> GetByIdAsync(int id);
    Task<bool> PostExistsAsync(int postId);
    Task<bool> HasUserReportedAsync(int postId, string userId);
    Task AddAsync(Report report);
    Task SaveChangesAsync();
}