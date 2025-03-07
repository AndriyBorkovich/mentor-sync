using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MentorSync.Notifications.Domain.Entities;

public sealed class EmailOutbox
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string To { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime? DateTimeUtcProcessed { get; set; } = null!;
}
