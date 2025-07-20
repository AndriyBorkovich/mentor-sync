using MediatR;

namespace MentorSync.SharedKernel.Abstractions.DomainEvents;

public class DomainEvent : INotification
{
    public DateTime DateOccurred { get; init; } = DateTime.UtcNow;
}
