namespace MentorSync.SharedKernel.Abstractions.Messaging.Notifications;

public interface INotificationHandler<in T>
	where T : INotification
{
	Task HandleAsync(T notification, CancellationToken cancellationToken = default);
}
