using MentorSync.SharedKernel.Abstractions.Messaging;
using IMediator = MediatR.IMediator;
using MentorSync.SharedKernel.Abstractions.DomainEvents;

namespace MentorSync.SharedKernel.Services;

public sealed class MediatorDomainEventsDispatcher (IMediator mediator) : IDomainEventsDispatcher
{
    public async Task DispatchAsync(IEnumerable<IHaveDomainEvents> entitiesWithEvents)
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
