using System.Text.Json;
using MediatR;
using MentorSync.Notifications.Contracts;
using MentorSync.Notifications.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace MentorSync.Notifications.Features.SendNotification;

public class BookingStatusChangedEventHandler(
    IHubContext<NotificationHub> hubContext,
    ILogger<BookingStatusChangedEventHandler> logger) : INotificationHandler<BookingStatusChangedEvent>
{
    public async Task Handle(BookingStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var json = JsonSerializer.Serialize(notification.Notification);
            await hubContext.Clients.User(notification.RecipientId)
                .SendAsync("BookingStatusChanged", json, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send booking status changed notification via SignalR");
        }
    }
}
