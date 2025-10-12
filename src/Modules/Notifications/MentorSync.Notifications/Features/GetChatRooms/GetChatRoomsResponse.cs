namespace MentorSync.Notifications.Features.GetChatRooms;

/// <summary>
/// Represents the response containing information about a chat room.
/// </summary>
/// <remarks>
/// Used in the GetChatRooms feature to return chat room details.
/// </remarks>
/// <example>
/// <code>
/// var response = new GetChatRoomsResponse
/// {
///     Id = "room-123",
///     ParticipantId = 42,
///     CreatedAt = DateTime.UtcNow,
///     LastMessage = new ChatMessageDto { ... },
///     UnreadCount = 5
/// };
/// </code>
/// </example>
public sealed record GetChatRoomsResponse
{
	/// <summary>
	/// Gets or sets the unique identifier of the chat room.
	/// </summary>
	public string Id { get; set; }

	/// <summary>
	/// Gets or sets the identifier of the participant in the chat room.
	/// </summary>
	public int ParticipantId { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the chat room was created.
	/// </summary>
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// Gets or sets the last message sent in the chat room.
	/// </summary>
	public ChatMessageDto LastMessage { get; set; }

	/// <summary>
	/// Gets or sets the number of unread messages in the chat room.
	/// </summary>
	public int UnreadCount { get; set; }
}

/// <summary>
/// Represents a chat message within a chat room.
/// </summary>
/// <example>
/// <code>
/// var message = new ChatMessageDto
/// {
///     Id = "msg-456",
///     SenderId = 7,
///     Content = "Hello!",
///     CreatedAt = DateTime.UtcNow,
///     IsRead = false
/// };
/// </code>
/// </example>
public sealed record ChatMessageDto
{
	/// <summary>
	/// Gets or sets the unique identifier of the message.
	/// </summary>
	public string Id { get; set; }

	/// <summary>
	/// Gets or sets the identifier of the sender.
	/// </summary>
	public int SenderId { get; set; }

	/// <summary>
	/// Gets or sets the content of the message.
	/// </summary>
	public string Content { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the message was created.
	/// </summary>
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the message has been read.
	/// </summary>
	public bool IsRead { get; set; }
}
