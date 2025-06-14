using MediatR;

namespace MentorSync.Notifications.Contracts;

public record BookingStatusChangedEvent(
    BookingStatusChangedNotification Notification,
    string RecipientId) : INotification;

public record BookingStatusChangedNotification(
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
