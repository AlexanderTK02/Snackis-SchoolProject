using Microsoft.EntityFrameworkCore;
using Snackis.Models;
using Snackis.Data;

namespace Snackis.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly ApplicationDbContext _context;

    public ProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfile?> GetByUserIdAsync(string userId)
    {
        return await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task SaveAsync(UserProfile profile)
    {
        var exists = await _context.UserProfiles.AnyAsync(p => p.UserId == profile.UserId);
        if (exists)
            _context.UserProfiles.Update(profile);
        else
            _context.UserProfiles.Add(profile);

        await _context.SaveChangesAsync();
    }
}
