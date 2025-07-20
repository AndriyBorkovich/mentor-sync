namespace MentorSync.SharedKernel.Abstractions.DomainEvents;

public interface IHaveDomainEvents
{
    public IEnumerable<DomainEvent> DomainEvents { get; }
    public void ClearDomainEvents();
    protected void RaiseDomainEvent(DomainEvent domainEvent);
}
