namespace MentorSync.Notifications.Contracts.Models;

/// <summary>
/// Command for sending email notifications
/// </summary>
public sealed class SendEmailCommand : ICommand<string>
{
	/// <summary>
	/// Gets or sets the recipient email address
	/// </summary>
	public string To { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the sender email address
	/// </summary>
	public string From { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the email subject
	/// </summary>
	public string Subject { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the email body content
	/// </summary>
	public string Body { get; set; } = string.Empty;
}
