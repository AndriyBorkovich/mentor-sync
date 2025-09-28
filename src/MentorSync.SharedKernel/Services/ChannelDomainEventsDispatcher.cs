using MentorSync.SharedKernel.Abstractions.DomainEvents;

namespace MentorSync.SharedKernel.Services;

/// <summary>
/// Dispatcher for publishing domain events through a channel-based publisher
/// </summary>
/// <param name="publisher">The publisher for domain events</param>
public sealed class ChannelDomainEventsDispatcher(IPublisher<DomainEvent> publisher) : IDomainEventsDispatcher
{
	/// <summary>
	/// Dispatches all domain events from the provided entities
	/// </summary>
	/// <param name="entitiesWithEvents">Entities containing domain events to dispatch</param>
	/// <returns>A task representing the asynchronous operation</returns>
	public async Task DispatchAsync(IEnumerable<IHaveDomainEvents> entitiesWithEvents)
	{
		foreach (var entity in entitiesWithEvents)
		{
			// grab & clear first so handlers can’t re-raise the same event
			var events = entity.DomainEvents.ToList();
			entity.ClearDomainEvents();

			foreach (var @event in events)
			{
				await publisher.PublishAsync(@event);
			}
		}
	}
}
