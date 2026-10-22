namespace Snackis.Models;

public class ConversationSummary
{
    public string OtherUserId { get; set; } = string.Empty;
    public string OtherUserName { get; set; } = string.Empty;
    public string LastText { get; set; } = string.Empty;
    public DateTime LastDate { get; set; }
    public bool LastFromMe { get; set; }
    public int UnreadCount { get; set; }
}

public class ConversationViewModel
{
    public string OtherUserId { get; set; } = string.Empty;
    public string OtherUserName { get; set; } = string.Empty;
    public List<Message> Messages { get; set; } = new();
}