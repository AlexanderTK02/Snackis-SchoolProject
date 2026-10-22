using Snackis.Models;

namespace Snackis.Services;

public interface ICommentService
{
    Task<List<Comment>> GetCommentsForPostAsync(int postId);
    Task AddCommentAsync(int postId, string Text, string userId);
}
