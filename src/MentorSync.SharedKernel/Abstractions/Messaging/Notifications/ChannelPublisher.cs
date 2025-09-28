using System.Threading.Channels;

namespace MentorSync.SharedKernel.Abstractions.Messaging.Notifications;

/// <summary>
/// Channel-based publisher/subscriber implementation for notifications
/// </summary>
/// <typeparam name="T">The type of notification to publish</typeparam>
public sealed class ChannelPubSub<T> : IPublisher<T>
	where T : INotification
{
	// Unbounded so publishers never block
	private readonly Channel<T> _channel = Channel.CreateUnbounded<T>();

	/// <summary>
	/// Gets the channel reader for consuming published notifications
	/// </summary>
	public ChannelReader<T> Reader => _channel.Reader;

	/// <summary>
	/// Publishes a notification to the channel
	/// </summary>
	/// <param name="notification">The notification to publish</param>
	/// <param name="cancellationToken">Cancellation token for the operation</param>
	/// <returns>A value task representing the asynchronous publish operation</returns>
	public ValueTask PublishAsync(T notification, CancellationToken cancellationToken = default)
		=> _channel.Writer.WriteAsync(notification, cancellationToken);
}
