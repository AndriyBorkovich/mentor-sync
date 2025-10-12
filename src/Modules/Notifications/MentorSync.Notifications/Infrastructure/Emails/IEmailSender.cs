namespace MentorSync.Notifications.Infrastructure.Emails;

/// <summary>
/// Service for sending emails
/// </summary>
public interface IEmailSender
{
	/// <summary>
	/// Sends an email asynchronously
	/// </summary>
	/// <param name="to">The recipient email address.</param>
	/// <param name="from">The sender email address.</param>
	/// <param name="subject">The subject of the email.</param>
	/// <param name="body">The body content of the email.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <example>
	/// <code>
	/// await emailSender.SendAsync("recipient@example.com", "sender@example.com", "Subject", "Body text");
	/// </code>
	/// </example>
	Task SendAsync(string to, string from, string subject, string body);
}
