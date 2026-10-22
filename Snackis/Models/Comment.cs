using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Snackis.Models;

public class Comment
{
    public int Id { get; set; }

    [Required]
    public string Text { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int PostId { get; set; }
    public Post? Post { get; set; }

    public string? UserId { get; set; }
    public IdentityUser? User { get; set; }
}
