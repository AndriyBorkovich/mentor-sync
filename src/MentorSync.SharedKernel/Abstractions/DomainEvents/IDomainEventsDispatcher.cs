namespace MentorSync.SharedKernel.Abstractions.DomainEvents;

/// <summary>
/// Interface for dispatching domain events from entities
/// </summary>
public interface IDomainEventsDispatcher
{
	/// <summary>
	/// Dispatches domain events from the provided entities
	/// </summary>
	/// <param name="entitiesWithEvents">The entities containing domain events to dispatch</param>
	/// <returns>A task representing the asynchronous dispatch operation</returns>
	Task DispatchAsync(IEnumerable<IHaveDomainEvents> entitiesWithEvents);
}
