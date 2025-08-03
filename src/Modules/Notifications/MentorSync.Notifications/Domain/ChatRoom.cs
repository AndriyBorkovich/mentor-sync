using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MentorSync.Notifications.Domain;

public sealed class ChatRoom
{
	[BsonId]
	public ObjectId Id { get; set; }

	/// <summary>
	/// Id of the first participant in the chat room
	/// </summary>
	[BsonElement("FistParticipant1Id")]
	public int FirstParticipantId { get; set; }

	/// <summary>
	/// Id of the second participant in the chat room
	/// </summary>
	[BsonElement("SecondParticipantId")]
	public int SecondParticipantId { get; set; }

	/// <summary>
	/// Timestamp of when the chat room was created
	/// </summary>
	[BsonElement("CreatedAt")]
	[BsonRepresentation(BsonType.DateTime)]
	public DateTime CreatedAt { get; set; }
}