using Snackis.Models;
using Snackis.Repositories;

namespace Snackis.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _repository;

    public ReportService(IReportRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Report>> GetAllReportsAsync()
    {
        return _repository.GetAllAsync();
    }

    // Returns null on success, or an error message
    public async Task<string?> ReportPostAsync(int postId, string userId, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            return "Please write a reason.";
        if (reason.Length > 500)
            return "The reason can be at most 500 characters.";
        if (!await _repository.PostExistsAsync(postId))
            return "That post doesn't exist.";
        if (await _repository.HasUserReportedAsync(postId, userId))
            return "You have already reported this post.";

        await _repository.AddAsync(new Report
        {
            PostId = postId,
            UserId = userId,
            Reason = reason.Trim(),
            CreatedAt = DateTime.Now
        });
        return null;
    }

    public async Task MarkReviewedAsync(int reportId)
    {
        var report = await _repository.GetByIdAsync(reportId);
        if (report == null) return;

        report.IsReviewed = true;
        await _repository.SaveChangesAsync();
    }
}