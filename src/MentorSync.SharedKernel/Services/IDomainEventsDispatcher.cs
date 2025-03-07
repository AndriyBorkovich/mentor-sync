using MentorSync.SharedKernel.Interfaces;

namespace MentorSync.SharedKernel.Services;

public interface IDomainEventsDispatcher
{
    Task DispatchAndClearEvents(IEnumerable<IHaveDomainEvents> entitiesWithEvents);
}
