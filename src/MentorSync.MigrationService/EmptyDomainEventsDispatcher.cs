using MentorSync.SharedKernel.Abstractions.DomainEvents;

namespace MentorSync.MigrationService;

internal sealed class EmptyDomainEventsDispatcher : IDomainEventsDispatcher
{
	public Task DispatchAsync(IEnumerable<IHaveDomainEvents> entitiesWithEvents) => Task.CompletedTask;
}
