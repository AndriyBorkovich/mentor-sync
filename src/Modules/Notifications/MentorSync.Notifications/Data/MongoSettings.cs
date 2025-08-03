namespace MentorSync.Notifications.Data;

public sealed class MongoSettings
{
	public string Database { get; set; } = default!;

	public string OutboxCollection { get; set; } = default!;
	public string RoomsCollection { get; set; } = default!;
	public string MessagesCollection { get; set; } = default!;
}
