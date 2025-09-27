using MentorSync.SharedKernel.Abstractions.DomainEvents;

namespace MentorSync.MigrationService;

public sealed class EmptyDomainEventsDispatcher : IDomainEventsDispatcher
{
	public Task DispatchAsync(IEnumerable<IHaveDomainEvents> entitiesWithEvents) => Task.CompletedTask;
}
