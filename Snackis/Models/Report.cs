using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Snackis.Models;

public class Report
{
    public int Id { get; set; }

    public int PostId { get; set; }
    public Post? Post { get; set; }

    public string? UserId { get; set; }
    public IdentityUser? User { get; set; }

    [Required, StringLength(500)]
    public string Reason { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsReviewed { get; set; }
}