using Snackis.Models;

namespace Snackis.Repositories;

public interface IProfileRepository
{
    Task<UserProfile?> GetByUserIdAsync(string userId);
    Task SaveAsync(UserProfile profile);
}
