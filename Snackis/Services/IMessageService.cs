using Snackis.Models;

namespace Snackis.Services;

public interface IMessageService
{
    Task<List<ConversationSummary>> GetConversationsAsync(string userId);
    Task<ConversationViewModel?> GetConversationAsync(string userId, string otherUserId);
    Task<int> CountUnreadAsync(string userId);
    Task<string?> FindUserIdAsync(string userName);
    Task<string?> SendAsync(string senderId, string recipientId, string text);
}