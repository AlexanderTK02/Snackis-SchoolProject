using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Snackis.Models;

public class UserProfile
{
    [Key]
    public string UserId { get; set; } = string.Empty;
    public IdentityUser? User { get; set; }

    public byte[]? ImageData { get; set; }
    public string? ImageContentType { get; set; }
}
