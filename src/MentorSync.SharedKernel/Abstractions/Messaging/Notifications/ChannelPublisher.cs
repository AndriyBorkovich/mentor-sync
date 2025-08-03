using System.Threading.Channels;

namespace MentorSync.SharedKernel.Abstractions.Messaging.Notifications;
public class ChannelPubSub<T> : IPublisher<T>
	where T : INotification
{
	// Unbounded so publishers never block
	private readonly Channel<T> _channel = Channel.CreateUnbounded<T>();

	public ChannelReader<T> Reader => _channel.Reader;

	public ValueTask PublishAsync(T notification, CancellationToken cancellationToken = default)
		=> _channel.Writer.WriteAsync(notification, cancellationToken);
}
