using System.Text.Json;
using Ardalis.Result;
using MentorSync.Notifications.Contracts.Models;
using MentorSync.Notifications.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace MentorSync.Notifications.Features.SendNotification;

/// <summary>
/// Handler for sending booking status changed notifications via SignalR
/// </summary>
/// <param name="hubContext"></param>
/// <param name="logger"></param>
public sealed class SendBookingStatusChangedNotificationCommandHandler(
	IHubContext<NotificationHub> hubContext,
	ILogger<SendBookingStatusChangedNotificationCommandHandler> logger)
		: ICommandHandler<SendBookingStatusChangedNotificationCommand, bool>
{
	/// <inheritdoc />
	public async Task<Result<bool>> Handle(
		SendBookingStatusChangedNotificationCommand request, CancellationToken cancellationToken = default)
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
