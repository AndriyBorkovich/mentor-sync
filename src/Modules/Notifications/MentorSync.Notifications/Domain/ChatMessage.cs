using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MentorSync.Notifications.Domain;

public sealed class ChatMessage
{
    [BsonId]
    public ObjectId Id { get; set; }

    /// <summary>
    /// Identifier of the chat room where the message was sent
    /// </summary>
    [BsonElement("RoomId")]
    public string RoomId { get; set; }

    /// <summary>
    /// Identifier of the sender of the chat message
    /// </summary>
    [BsonElement("SenderId")]
    public int SenderId { get; set; }

    /// <summary>
    /// Content of the chat message
    /// </summary>
    [BsonElement("Content")]
    public string Content { get; set; } = null!;

    /// <summary>
    /// Timestamp of when the message was created
    /// </summary> 
    [BsonElement("CreatedAt")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Indicates whether the message has been read by the recipient
    /// </summary>
    [BsonElement("IsRead")]
    [BsonRepresentation(BsonType.Boolean)]
    public bool IsRead { get; set; }

    /// <summary>
    /// Indicates whether the message has been edited
    /// </summary>
    [BsonElement("IsEdited")]
    [BsonRepresentation(BsonType.Boolean)]
    public bool IsEdited { get; set; } = false;
}