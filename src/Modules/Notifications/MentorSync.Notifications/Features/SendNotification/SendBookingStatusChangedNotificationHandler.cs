using System.Text.Json;
using MediatR;
using MentorSync.Notifications.Contracts;
using MentorSync.Notifications.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace MentorSync.Notifications.Features.SendNotification;

public class SendBookingStatusChangedNotificationHandler(
    IHubContext<NotificationHub> hubContext,
    ILogger<SendBookingStatusChangedNotificationHandler> logger) : IRequestHandler<SendBookingStatusChangedNotificationCommand, bool>
{
    public async Task<bool> Handle(SendBookingStatusChangedNotificationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var notificationJson = JsonSerializer.Serialize(request.Notification);
            await hubContext.Clients.User(request.UserId)
                .SendAsync("BookingStatusChanged", notificationJson, cancellationToken);

            logger.LogInformation(
                "Sent booking status changed notification for booking {BookingId} to user {UserId}",
                request.Notification.BookingId, request.UserId);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error sending booking status changed notification for booking {BookingId} to user {UserId}",
                request.Notification.BookingId, request.UserId);
            return false;
        }
    }
}
