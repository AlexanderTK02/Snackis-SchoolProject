using Snackis.Models;

namespace Snackis.Repositories;

public interface ICommentRepository
{
    Task<List<Comment>> GetByPostIdAsync(int postId);
    Task AddAsync(Comment comment);
}
