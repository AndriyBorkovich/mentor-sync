namespace MentorSync.Notifications.Contracts;

public sealed class SendEmailCommand : ICommand<string>
{
	public string To { get; set; } = string.Empty;
	public string From { get; set; } = string.Empty;
	public string Subject { get; set; } = string.Empty;
	public string Body { get; set; } = string.Empty;
}
