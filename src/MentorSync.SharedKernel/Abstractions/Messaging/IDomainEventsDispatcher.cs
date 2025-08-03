using MentorSync.SharedKernel.Abstractions.DomainEvents;

namespace MentorSync.SharedKernel.Abstractions.Messaging;

public interface IDomainEventsDispatcher
{
	Task DispatchAsync(IEnumerable<IHaveDomainEvents> entitiesWithEvents);
}
