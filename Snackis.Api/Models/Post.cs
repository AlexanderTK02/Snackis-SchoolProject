namespace Snackis.Api.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public string? UserId { get; set; }
    public ForumUser? User { get; set; }
}