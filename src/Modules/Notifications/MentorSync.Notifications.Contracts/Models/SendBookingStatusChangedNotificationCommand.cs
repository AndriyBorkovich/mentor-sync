namespace MentorSync.Notifications.Contracts.Models;

/// <summary>
/// Command to send a booking status changed notification
/// </summary>
public sealed class SendBookingStatusChangedNotificationCommand : ICommand<bool>
{
	/// <summary>
	/// Details of the booking status changed notification
	/// </summary>
	public required BookingStatusChangedNotification Notification { get; init; }
	/// <summary>
	/// Identifier of the user to whom the notification is to be sent
	/// </summary>
	public required string UserId { get; init; }
}
