namespace MentorSync.SharedKernel.Abstractions.DomainEvents;

public interface IDomainEventsDispatcher
{
	Task DispatchAsync(IEnumerable<IHaveDomainEvents> entitiesWithEvents);
}
