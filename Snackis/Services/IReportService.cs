using Snackis.Models;

namespace Snackis.Services;

public interface IReportService
{
    Task<List<Report>> GetAllReportsAsync();
    Task<string?> ReportPostAsync(int postId, string userId, string reason);
    Task MarkReviewedAsync(int reportId);
}