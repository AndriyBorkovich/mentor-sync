namespace MentorSync.Notifications.Contracts.Models;

/// <summary>
/// Event representing a change in booking status
/// </summary>
/// <param name="Notification"></param>
/// <param name="RecipientId"></param>
public sealed record BookingStatusChangedEvent(
	BookingStatusChangedNotification Notification,
	string RecipientId) : INotification;

/// <summary>
/// Notification details for a booking status change
/// </summary>
/// <param name="BookingId"></param>
/// <param name="NewStatus"></param>
/// <param name="Title"></param>
/// <param name="StartTime"></param>
/// <param name="EndTime"></param>
/// <param name="MentorName"></param>
/// <param name="StudentName"></param>
/// <param name="Message"></param>
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
	/// <summary>
	/// The name of the notification type
	/// </summary>
	public static readonly string NotificationTypeName = nameof(BookingStatusChangedNotification);
}
