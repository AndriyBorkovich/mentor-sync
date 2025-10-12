namespace MentorSync.Notifications.Features.InitiateChat;

/// <summary>
/// Response model for initiating a chat session
/// </summary>
public sealed class InitiateChatResponse
{
	/// <summary>
	/// Unique identifier of the initiated chat room
	/// </summary>
	public string ChatRoomId { get; set; }
}
