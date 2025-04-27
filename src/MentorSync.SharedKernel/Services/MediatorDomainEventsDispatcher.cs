using MediatR;
using MentorSync.SharedKernel.Interfaces;

namespace MentorSync.SharedKernel.Services;

public sealed class MediatorDomainEventsDispatcher (IMediator mediator) : IDomainEventsDispatcher
{
    public async Task DispatchAndClearEvents(IEnumerable<IHaveDomainEvents> entitiesWithEvents)
    {
        foreach (var entity in entitiesWithEvents)
        {
            var domainEvents = entity.DomainEvents.ToList();
            entity.ClearDomainEvents();
            foreach (var @event in domainEvents)
            {
                await mediator.Publish(@event);
            }
        }
    }
}
