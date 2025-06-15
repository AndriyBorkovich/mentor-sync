namespace MentorSync.Notifications.Features.GetChatRooms;

public sealed class GetChatRoomsResponse
{
    public string Id { get; set; }
    public int ParticipantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public ChatMessageDto LastMessage { get; set; }
    public int UnreadCount { get; set; }
}

public sealed class ChatMessageDto
{
    public string Id { get; set; }
    public int SenderId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
}
