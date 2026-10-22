using Microsoft.EntityFrameworkCore;
using Snackis.Data;
using Snackis.Models;

namespace Snackis.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ApplicationDbContext _context;

    public MessageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Message>> GetAllForUserAsync(string userId)
    {
        return await _context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Recipient)
            .Where(m => m.SenderId == userId || m.RecipientId == userId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Message>> GetConversationAsync(string userId, string otherUserId)
    {
        return await _context.Messages
            .Where(m => (m.SenderId == userId && m.RecipientId == otherUserId)
                     || (m.SenderId == otherUserId && m.RecipientId == userId))
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task MarkConversationReadAsync(string userId, string otherUserId)
    {
        await _context.Messages
            .Where(m => m.RecipientId == userId && m.SenderId == otherUserId && !m.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(m => m.IsRead, true));
    }

    public async Task<int> CountUnreadAsync(string userId)
    {
        return await _context.Messages.CountAsync(m => m.RecipientId == userId && !m.IsRead);
    }

    public async Task<string?> FindUserIdByUserNameAsync(string userName)
    {
        var normalized = userName.ToUpperInvariant();
        return await _context.Users
            .Where(u => u.NormalizedUserName == normalized)
            .Select(u => u.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<string?> FindUserNameByIdAsync(string userId)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.UserName)
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(Message message)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
    }
}