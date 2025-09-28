namespace MentorSync.SharedKernel.Abstractions.DomainEvents;

/// <summary>
/// Base class for domain events that represent something important that happened in the domain
/// </summary>
public class DomainEvent : INotification
{
	/// <summary>
	/// Gets the date and time when the domain event occurred
	/// </summary>
	public DateTime DateOccurred { get; init; } = DateTime.UtcNow;
}
