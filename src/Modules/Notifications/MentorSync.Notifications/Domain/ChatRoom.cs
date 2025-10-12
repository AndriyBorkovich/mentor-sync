using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MentorSync.Notifications.Domain;

/// <summary>
/// Represents a chat room between two participants
/// </summary>
public sealed class ChatRoom
{
	/// <summary>
	/// Unique identifier for the chat room
	/// </summary>
	[BsonId]
	public ObjectId Id { get; set; }

	/// <summary>
	/// Identifier of the first participant in the chat room
	/// </summary>
	[BsonElement("FistParticipant1Id")]
	public int FirstParticipantId { get; init; }

	/// <summary>
	/// Identifier of the second participant in the chat room
	/// </summary>
	[BsonElement("SecondParticipantId")]
	public int SecondParticipantId { get; init; }

	/// <summary>
	/// Timestamp of when the chat room was created
	/// </summary>
	[BsonElement("CreatedAt")]
	[BsonRepresentation(BsonType.DateTime)]
	public DateTime CreatedAt { get; init; }
}
