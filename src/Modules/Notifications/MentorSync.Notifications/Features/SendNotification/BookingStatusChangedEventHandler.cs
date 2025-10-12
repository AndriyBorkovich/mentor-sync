using System.Text.Json;
using MentorSync.Notifications.Contracts.Models;
using MentorSync.Notifications.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace MentorSync.Notifications.Features.SendNotification;

/// <summary>
/// Event handler for booking status change notifications
/// </summary>
/// <param name="hubContext"></param>
/// <param name="logger"></param>
public sealed class BookingStatusChangedEventHandler(
	IHubContext<NotificationHub> hubContext,
	ILogger<BookingStatusChangedEventHandler> logger) : INotificationHandler<BookingStatusChangedEvent>
{
	/// <inheritdoc />
	public async Task HandleAsync(BookingStatusChangedEvent notification, CancellationToken cancellationToken = default)
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
			throw;
		}
	}
}
