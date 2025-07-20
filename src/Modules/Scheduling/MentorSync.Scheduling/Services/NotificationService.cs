using MentorSync.Notifications.Contracts;
using Microsoft.Extensions.Logging;

namespace MentorSync.Scheduling.Services;

public interface INotificationService
{
    Task SendBookingStatusChangedNotificationAsync(
        int bookingId,
        string status,
        string title,
        DateTime startTime,
        DateTime endTime,
        int recipientId,
        string message = null);
}

public class NotificationService(MediatR.IMediator mediator,
    ILogger<NotificationService> logger) : INotificationService
{
    private readonly MediatR.IMediator _mediator = mediator;

    public async Task SendBookingStatusChangedNotificationAsync(
        int bookingId,
        string status,
        string title,
        DateTime startTime,
        DateTime endTime,
        int recipientId,
        string message = null)
    {
        try
        {
            var notification = new BookingStatusChangedNotification(
                BookingId: bookingId,
                NewStatus: status,
                Title: title,
                StartTime: startTime,
                EndTime: endTime,
                MentorName: null,
                StudentName: null,
                Message: message ?? "Your booking status has been updated"
            );

            // Use direct SignalR invocation
            await _mediator.Publish(
                new BookingStatusChangedEvent(
                    notification,
                    recipientId.ToString()
            ));
        }
        catch (Exception ex)
        {
            // Log exception but don't throw
            logger.LogError(ex, "Failed to send notification:");
        }
    }
}
