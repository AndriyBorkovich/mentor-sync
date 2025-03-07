using MentorSync.SharedKernel.BaseEntities;

namespace MentorSync.SharedKernel.Interfaces;

public interface IHaveDomainEvents
{
    public IEnumerable<DomainEvent> DomainEvents { get; }
    public void ClearDomainEvents();
    protected void RaiseDomainEvent(DomainEvent domainEvent);
}
