using MentorSync.SharedKernel.Abstractions.DomainEvents;
using MentorSync.SharedKernel.Abstractions.Messaging;

namespace MentorSync.MigrationService;

public sealed class EmptyDomainEventsDispatcher : IDomainEventsDispatcher
{
	public Task DispatchAsync(IEnumerable<IHaveDomainEvents> entitiesWithEvents) => Task.CompletedTask;
}
