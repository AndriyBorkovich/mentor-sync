namespace MentorSync.Notifications.Data;

/// <summary>
/// Configuration settings for MongoDB connection
/// </summary>
public sealed class MongoSettings
{
	/// <summary>
	/// Gets or sets the connection string for the MongoDB database
	/// </summary>
	public string Database { get; init; }
	/// <summary>
	/// Gets or sets the name of the MongoDB collection for outbox messages
	/// </summary>
	public string OutboxCollection { get; init; }
	/// <summary>
	///	 Gets or sets the name of the MongoDB collection for rooms
	/// </summary>
	public string RoomsCollection { get; init; }
	/// <summary>
	/// Gets or sets the name of the MongoDB collection for messages
	/// </summary>
	public string MessagesCollection { get; init; }
}
