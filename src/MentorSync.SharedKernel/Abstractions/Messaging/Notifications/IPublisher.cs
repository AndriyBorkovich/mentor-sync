namespace MentorSync.SharedKernel.Abstractions.Messaging.Notifications;

// <summary>Publishes notifications of type T.</summary>
public interface IPublisher<in T>
	where T : INotification
{
	ValueTask PublishAsync(T notification, CancellationToken cancellationToken = default);
}
