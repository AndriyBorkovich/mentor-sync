using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MentorSync.Notifications.Domain;

public sealed class EmailOutbox
{
    [BsonId]
    public ObjectId Id { get; set; }

    /// <summary>
    /// The recipient email address for the email
    /// </summary>
    [BsonElement("To")]
    public string To { get; set; } = string.Empty;

    /// <summary>
    /// The sender email address for the email
    /// </summary>
    [BsonElement("From")]
    public string From { get; set; } = string.Empty;

    /// <summary>
    /// The subject line of the email
    /// </summary>
    [BsonElement("Subject")]
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// The body content of the email
    /// </summary>
    [BsonElement("Body")]
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp of when the email was actually sent by the system
    /// </summary>
    [BsonElement("DateTimeUtcProcessed")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime? DateTimeUtcProcessed { get; set; } = null!;
}
