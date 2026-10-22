using Snackis.Models;
using Snackis.Repositories;

namespace Snackis.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _repository;

    public CommentService(ICommentRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Comment>> GetCommentsForPostAsync(int postId)
    {
        return _repository.GetByPostIdAsync(postId);
    }

    public async Task AddCommentAsync(int postId, string text, string userId)
    {
        if (string.IsNullOrWhiteSpace(text)) return;

        var comment = new Comment
        {
            PostId = postId,
            Text = text.Trim(),
            UserId = userId,
            CreatedAt = DateTime.Now
        };
        await _repository.AddAsync(comment);
    }
}
