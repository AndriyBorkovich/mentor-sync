using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MentorSync.Notifications.Domain;

/// <summary>
/// Represents a chat message in the system
/// </summary>
public sealed class ChatMessage
{
	/// <summary>
	/// Unique identifier for the chat message
	/// </summary>
	[BsonId]
	public ObjectId Id { get; init; }

	/// <summary>
	/// Identifier of the chat room where the message was sent
	/// </summary>
	[BsonElement("RoomId")]
	public string RoomId { get; init; }

	/// <summary>
	/// Identifier of the sender of the chat message
	/// </summary>
	[BsonElement("SenderId")]
	public int SenderId { get; init; }

	/// <summary>
	/// Content of the chat message
	/// </summary>
	[BsonElement("Content")]
	public string Content { get; init; } = null!;

	/// <summary>
	/// Timestamp of when the message was created
	/// </summary>
	[BsonElement("CreatedAt")]
	[BsonRepresentation(BsonType.DateTime)]
	public DateTime CreatedAt { get; init; }

	/// <summary>
	/// Indicates whether the message has been read by the recipient
	/// </summary>
	[BsonElement("IsRead")]
	[BsonRepresentation(BsonType.Boolean)]
	public bool IsRead { get; init; }

	/// <summary>
	/// Indicates whether the message has been edited
	/// </summary>
	[BsonElement("IsEdited")]
	[BsonRepresentation(BsonType.Boolean)]
	public bool IsEdited { get; set; } = false;
}
