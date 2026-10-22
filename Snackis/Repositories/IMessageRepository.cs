using Snackis.Models;

namespace Snackis.Repositories;

public interface IMessageRepository
{
    Task<List<Message>> GetAllForUserAsync(string userId);
    Task<List<Message>> GetConversationAsync(string userId, string otherUserId);
    Task MarkConversationReadAsync(string userId, string otherUserId);
    Task<int> CountUnreadAsync(string userId);
    Task<string?> FindUserIdByUserNameAsync(string userName);
    Task<string?> FindUserNameByIdAsync(string userId);
    Task AddAsync(Message message);
}