using Microsoft.AspNetCore.Http;
using Snackis.Models;
using Snackis.Repositories;

namespace Snackis.Services;

public class ProfileService : IProfileService
{
    private const long MaxSizeBytes = 2 * 1024 * 1024;
    private static readonly string[] AllowedTypes =
        { "image/jpeg", "image/png", "image/gif", "image/webp" };
    private readonly IProfileRepository _repository;

    public ProfileService(IProfileRepository repository)
    {
        _repository = repository;
    }

    public Task<UserProfile?> GetProfileAsync(string userId)
    {
        return _repository.GetByUserIdAsync(userId);
    }

    // Returns null on success, or an error message
    public async Task<string?> SaveImageAsync(string userId, IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return "Please choose a picture.";
        if (file.Length > MaxSizeBytes)
            return "The picture must be smaller than 2 MB.";
        if (!AllowedTypes.Contains(file.ContentType))
            return "Only JPG, PNG, GIF or WebP pictures are allowed.";

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        var profile = await _repository.GetByUserIdAsync(userId)
                                ?? new UserProfile { UserId = userId };
        profile.ImageData = stream.ToArray();
        profile.ImageContentType = file.ContentType;

        await _repository.SaveAsync(profile);
        return null;
    }
}
