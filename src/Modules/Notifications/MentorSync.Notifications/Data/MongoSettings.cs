namespace MentorSync.Notifications.Data;

public sealed class MongoSettings
{
    public string Database { get; set; } = default!;

    public string Collection { get; set; } = default!;
}
