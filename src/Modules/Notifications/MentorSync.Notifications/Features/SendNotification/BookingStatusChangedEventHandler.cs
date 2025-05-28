using System.Text.Json;
using MediatR;
using MentorSync.Notifications.Contracts;
using MentorSync.Notifications.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace MentorSync.Notifications.Features.SendNotification;

public class BookingStatusChangedEventHandler : INotificationHandler<BookingStatusChangedEvent>
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<BookingStatusChangedEventHandler> _logger;

    public BookingStatusChangedEventHandler(
        IHubContext<NotificationHub> hubContext,
        ILogger<BookingStatusChangedEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task Handle(BookingStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var json = JsonSerializer.Serialize(notification.Notification);
            await _hubContext.Clients.User(notification.RecipientId)
                .SendAsync("BookingStatusChanged", json, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send booking status changed notification via SignalR");
        }
    }
}
