using System.Text.Json;
using MediatR;
using MentorSync.Notifications.Contracts;
using MentorSync.Notifications.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace MentorSync.Notifications.Features.SendNotification;

public class SendBookingStatusChangedNotificationHandler : IRequestHandler<SendBookingStatusChangedNotificationCommand, bool>
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<SendBookingStatusChangedNotificationHandler> _logger;

    public SendBookingStatusChangedNotificationHandler(
        IHubContext<NotificationHub> hubContext,
        ILogger<SendBookingStatusChangedNotificationHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task<bool> Handle(SendBookingStatusChangedNotificationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var notificationJson = JsonSerializer.Serialize(request.Notification);
            await _hubContext.Clients.User(request.UserId)
                .SendAsync("BookingStatusChanged", notificationJson, cancellationToken);

            _logger.LogInformation(
                "Sent booking status changed notification for booking {BookingId} to user {UserId}",
                request.Notification.BookingId, request.UserId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error sending booking status changed notification for booking {BookingId} to user {UserId}",
                request.Notification.BookingId, request.UserId);
            return false;
        }
    }
}
