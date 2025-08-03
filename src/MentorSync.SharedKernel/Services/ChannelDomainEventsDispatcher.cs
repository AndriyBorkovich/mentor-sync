using MentorSync.SharedKernel.Abstractions.DomainEvents;

namespace MentorSync.SharedKernel.Services;

public sealed class ChannelDomainEventsDispatcher(IPublisher<DomainEvent> publisher) : IDomainEventsDispatcher
{
	private readonly IPublisher<DomainEvent> _publisher = publisher;

	public async Task DispatchAsync(IEnumerable<IHaveDomainEvents> entitiesWithEvents)
	{
		foreach (var entity in entitiesWithEvents)
		{
			// grab & clear first so handlers can’t re-raise the same event
			var events = entity.DomainEvents.ToList();
			entity.ClearDomainEvents();

			foreach (var @event in events)
			{
				await _publisher.PublishAsync(@event);
			}
		}
	}
}
