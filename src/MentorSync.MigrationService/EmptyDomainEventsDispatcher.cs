using MentorSync.SharedKernel.Interfaces;
using MentorSync.SharedKernel.Services;

namespace MentorSync.MigrationService;

public sealed class EmptyDomainEventsDispatcher : IDomainEventsDispatcher
{
    public Task DispatchAndClearEvents(IEnumerable<IHaveDomainEvents> entitiesWithEvents) => Task.CompletedTask;
}
