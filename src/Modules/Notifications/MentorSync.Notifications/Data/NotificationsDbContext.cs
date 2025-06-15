using MentorSync.Notifications.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MentorSync.Notifications.Data;

public sealed class NotificationsDbContext(IMongoClient mongoClient, IOptions<MongoSettings> settings)
{
    private readonly IMongoDatabase _database = mongoClient.GetDatabase(settings.Value.Database);

    public IMongoCollection<EmailOutbox> EmailOutboxes => _database.GetCollection<EmailOutbox>(settings.Value.OutboxCollection);
    public IMongoCollection<ChatRoom> ChatRooms => _database.GetCollection<ChatRoom>(settings.Value.RoomsCollection);
    public IMongoCollection<ChatMessage> ChatMessages => _database.GetCollection<ChatMessage>(settings.Value.MessagesCollection);
}
