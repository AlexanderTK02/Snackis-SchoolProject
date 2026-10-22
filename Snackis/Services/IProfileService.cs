using Microsoft.AspNetCore.Http;
using Snackis.Models;

namespace Snackis.Services;

public interface IProfileService
{
    Task<UserProfile?> GetProfileAsync(string userId);
    Task<string?> SaveImageAsync(string userId, IFormFile? file);
}
