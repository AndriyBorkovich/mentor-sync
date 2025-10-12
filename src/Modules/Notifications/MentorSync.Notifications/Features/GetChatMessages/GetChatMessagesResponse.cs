namespace MentorSync.Notifications.Features.GetChatMessages;

/// <summary>
/// Response model for retrieving chat messages
/// </summary>
public sealed class GetChatMessagesResponse
{
	/// <summary>
	/// Unique identifier of the chat message
	/// </summary>
	public string Id { get; set; }
	/// <summary>
	/// Identifier of the sender of the chat message
	/// </summary>
	public int SenderId { get; set; }
	/// <summary>
	/// Identifier of the receiver of the chat message
	/// </summary>
	public int ReceiverId { get; set; }
	/// <summary>
	/// Content of the chat message
	/// </summary>
	public string Content { get; set; }
	/// <summary>
	/// Timestamp of when the message was created
	/// </summary>
	public DateTime Timestamp { get; set; }
	/// <summary>
	/// Indicates whether the message has been read by the recipient
	/// </summary>
	public bool IsRead { get; set; }
}
