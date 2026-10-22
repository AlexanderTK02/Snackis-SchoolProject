using Snackis.Models;
using Snackis.Repositories;

namespace Snackis.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _repository;

    public MessageService(IMessageRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ConversationSummary>> GetConversationsAsync(string userId)
    {
        var messages = await _repository.GetAllForUserAsync(userId);

        return messages
            .GroupBy(m => m.SenderId == userId ? m.RecipientId : m.SenderId)
            .Where(g => g.Key != null)
            .Select(g =>
            {
                var last = g.OrderByDescending(m => m.CreatedAt).First();
                var other = last.SenderId == userId ? last.Recipient : last.Sender;

                return new ConversationSummary
                {
                    OtherUserId = g.Key!,
                    OtherUserName = other?.UserName ?? "Unknown",
                    LastText = last.Text,
                    LastDate = last.CreatedAt,
                    LastFromMe = last.SenderId == userId,
                    UnreadCount = g.Count(m => m.RecipientId == userId && !m.IsRead)
                };
            })
            .OrderByDescending(c => c.LastDate)
            .ToList();
    }

    public async Task<ConversationViewModel?> GetConversationAsync(string userId, string otherUserId)
    {
        if (userId == otherUserId) return null;

        var otherName = await _repository.FindUserNameByIdAsync(otherUserId);
        if (otherName == null) return null;

        await _repository.MarkConversationReadAsync(userId, otherUserId);
        var messages = await _repository.GetConversationAsync(userId, otherUserId);

        return new ConversationViewModel
        {
            OtherUserId = otherUserId,
            OtherUserName = otherName,
            Messages = messages
        };
    }

    public Task<int> CountUnreadAsync(string userId) => _repository.CountUnreadAsync(userId);

    public Task<string?> FindUserIdAsync(string userName) =>
        _repository.FindUserIdByUserNameAsync(userName.Trim());

    // Returns null on success, or an error message
    public async Task<string?> SendAsync(string senderId, string recipientId, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "Please write a message.";
        if (text.Length > 2000)
            return "The message can be at most 2000 characters.";
        if (recipientId == senderId)
            return "You can't send a message to yourself.";
        if (await _repository.FindUserNameByIdAsync(recipientId) == null)
            return "That user doesn't exist.";

        await _repository.AddAsync(new Message
        {
            SenderId = senderId,
            RecipientId = recipientId,
            Text = text.Trim(),
            CreatedAt = DateTime.Now
        });
        return null;
    }
}