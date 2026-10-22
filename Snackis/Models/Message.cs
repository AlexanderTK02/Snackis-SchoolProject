using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Snackis.Models;

public class Message
{
    public int Id { get; set; }

    public string? SenderId { get; set; }
    public IdentityUser? Sender { get; set; }

    public string? RecipientId { get; set; }
    public IdentityUser? Recipient { get; set; }

    [Required, StringLength(2000)]
    public string Text { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsRead { get; set; }
}