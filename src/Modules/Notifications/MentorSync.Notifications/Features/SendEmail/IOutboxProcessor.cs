namespace MentorSync.Notifications.Features.SendEmail;

/// <summary>
/// Interface for processing outbox messages and sending emails
/// </summary>
public interface IOutboxProcessor
{
	/// <summary>
	/// Checks for emails to send and processes them
	/// </summary>
	/// <returns>Task</returns>
	Task CheckForEmailsToSend();
}
