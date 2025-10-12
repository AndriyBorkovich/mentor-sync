using System.Globalization;
using MentorSync.Notifications.Contracts.Models;
using Microsoft.Extensions.Logging;

namespace MentorSync.Scheduling.Services;

/// <summary>
/// Service to send notifications
/// </summary>
public interface INotificationService
{
	/// <summary>
	/// Send notification when booking status changes
	/// </summary>
	/// <param name="bookingId">Booking ID</param>
	/// <param name="status">New status</param>
	/// <param name="title">Booking title</param>
	/// <param name="startTime">Booking start time</param>
	/// <param name="endTime">Booking end time</param>
	/// <param name="recipientId">Recipient user ID</param>
	/// <param name="message">Optional custom message</param>
	/// <returns>A task that represents the asynchronous operation</returns>
	Task SendBookingStatusChangedNotificationAsync(
		int bookingId,
		string status,
		string title,
		DateTime startTime,
		DateTime endTime,
		int recipientId,
		string message = null);
}

/// <inheritdoc />
public sealed class NotificationService(
	IPublisher<BookingStatusChangedEvent> publisher,
	ILogger<NotificationService> logger) : INotificationService
{
	/// <inheritdoc />
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
