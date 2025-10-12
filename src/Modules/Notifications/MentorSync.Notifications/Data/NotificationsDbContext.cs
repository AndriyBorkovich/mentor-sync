using MentorSync.Notifications.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MentorSync.Notifications.Data;

/// <summary>
/// Database context for notifications module
/// </summary>
/// <param name="mongoClient">MongoDB connection service</param>
/// <param name="settings">Database settings</param>
public sealed class NotificationsDbContext(IMongoClient mongoClient, IOptions<MongoSettings> settings)
{
	private readonly IMongoDatabase _database = mongoClient.GetDatabase(settings.Value.Database);

	/// <summary>
	/// Gets the collection of email outbox entries
	/// </summary>
	public IMongoCollection<EmailOutbox> EmailOutboxes => _database.GetCollection<EmailOutbox>(settings.Value.OutboxCollection);
	/// <summary>
	/// Gets the collection of chat rooms
	/// </summary>
	public IMongoCollection<ChatRoom> ChatRooms => _database.GetCollection<ChatRoom>(settings.Value.RoomsCollection);
	/// <summary>
	/// Gets the collection of chat messages
	/// </summary>
	public IMongoCollection<ChatMessage> ChatMessages => _database.GetCollection<ChatMessage>(settings.Value.MessagesCollection);
}
