namespace MentorSync.SharedKernel.Abstractions.DomainEvents;

/// <summary>
/// Interface for entities that can raise domain events
/// </summary>
public interface IHaveDomainEvents
{
	/// <summary>
	/// Gets the collection of domain events raised by this entity
	/// </summary>
	public IEnumerable<DomainEvent> DomainEvents { get; }

	/// <summary>
	/// Clears all domain events from this entity
	/// </summary>
	public void ClearDomainEvents();

	/// <summary>
	/// Raises a domain event for this entity
	/// </summary>
	/// <param name="domainEvent">The domain event to raise</param>
	protected void RaiseDomainEvent(DomainEvent domainEvent);
}
