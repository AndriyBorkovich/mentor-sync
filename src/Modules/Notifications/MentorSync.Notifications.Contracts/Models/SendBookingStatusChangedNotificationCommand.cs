namespace MentorSync.Notifications.Contracts.Models;

public sealed class SendBookingStatusChangedNotificationCommand : ICommand<bool>
{
	public required BookingStatusChangedNotification Notification { get; init; }
	public required string UserId { get; init; }
}
