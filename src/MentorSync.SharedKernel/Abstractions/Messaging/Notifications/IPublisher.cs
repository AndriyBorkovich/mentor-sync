namespace MentorSync.SharedKernel.Abstractions.Messaging.Notifications;

/// <summary>
/// Publishes notifications of type T
/// </summary>
/// <typeparam name="T">The type of notification to publish</typeparam>
public interface IPublisher<in T>
	where T : INotification
{
	/// <summary>
	/// Publishes a notification asynchronously
	/// </summary>
	/// <param name="notification">The notification to publish</param>
	/// <param name="cancellationToken">Cancellation token for the operation</param>
	/// <returns>A value task representing the asynchronous publish operation</returns>
	ValueTask PublishAsync(T notification, CancellationToken cancellationToken = default);
}
