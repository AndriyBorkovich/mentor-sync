namespace MentorSync.Notifications.Contracts.Models;

public sealed record BookingStatusChangedEvent(
	BookingStatusChangedNotification Notification,
	string RecipientId) : INotification;

public sealed record BookingStatusChangedNotification(
	int BookingId,
	string NewStatus,
	string Title,
	DateTime StartTime,
	DateTime EndTime,
	string MentorName,
	string StudentName,
	string Message)
{
	public static readonly string NotificationTypeName = nameof(BookingStatusChangedNotification);
}
