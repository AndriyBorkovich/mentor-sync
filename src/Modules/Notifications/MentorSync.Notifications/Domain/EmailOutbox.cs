using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MentorSync.Notifications.Domain;

/// <summary>
/// Represents an email queued for sending in the outbox
/// </summary>
public sealed class EmailOutbox
{
	/// <summary>
	/// Unique identifier for the email outbox entry
	/// </summary>
	[BsonId]
	public ObjectId Id { get; init; }

	/// <summary>
	/// The recipient email address for the email
	/// </summary>
	[BsonElement("To")]
	public string To { get; init; } = string.Empty;

	/// <summary>
	/// The sender email address for the email
	/// </summary>
	[BsonElement("From")]
	public string From { get; init; } = string.Empty;

	/// <summary>
	/// The subject line of the email
	/// </summary>
	[BsonElement("Subject")]
	public string Subject { get; init; } = string.Empty;

	/// <summary>
	/// The body content of the email
	/// </summary>
	[BsonElement("Body")]
	public string Body { get; init; } = string.Empty;

	/// <summary>
	/// Timestamp of when the email was actually sent by the system
	/// </summary>
	[BsonElement("DateTimeUtcProcessed")]
	[BsonRepresentation(BsonType.DateTime)]
	public DateTime? DateTimeUtcProcessed { get; set; } = null!;
}
