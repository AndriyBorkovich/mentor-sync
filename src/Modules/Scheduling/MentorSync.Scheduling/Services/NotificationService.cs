using System.Globalization;
using MentorSync.Notifications.Contracts.Models;
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

public sealed class NotificationService(
	IPublisher<BookingStatusChangedEvent> publisher,
	ILogger<NotificationService> logger) : INotificationService
{
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

			await publisher.PublishAsync(
				new BookingStatusChangedEvent(
					notification,
					recipientId.ToString(CultureInfo.InvariantCulture)
			));
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to send notification");
			throw;
		}
	}
}
