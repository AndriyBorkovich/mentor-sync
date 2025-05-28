using MediatR;

namespace MentorSync.Notifications.Contracts;

public class SendBookingStatusChangedNotificationCommand : IRequest<bool>
{
    public required BookingStatusChangedNotification Notification { get; init; }
    public required string UserId { get; init; }
}
