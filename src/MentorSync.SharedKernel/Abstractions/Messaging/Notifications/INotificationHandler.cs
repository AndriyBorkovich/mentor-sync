namespace MentorSync.SharedKernel.Abstractions.Messaging.Notifications;

/// <summary>
/// Interface for handling notifications of a specific type
/// </summary>
/// <typeparam name="T">The type of notification to handle</typeparam>
public interface INotificationHandler<in T>
	where T : INotification
{
	/// <summary>
	/// Handles the specified notification
	/// </summary>
	/// <param name="notification">The notification to handle</param>
	/// <param name="cancellationToken">Cancellation token for the operation</param>
	/// <returns>A task representing the asynchronous handle operation</returns>
	Task HandleAsync(T notification, CancellationToken cancellationToken = default);
}
