namespace MentorSync.Notifications.Features.GetChatMessages;

public sealed class GetChatMessagesResponse
{
    public string Id { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsRead { get; set; }
}
