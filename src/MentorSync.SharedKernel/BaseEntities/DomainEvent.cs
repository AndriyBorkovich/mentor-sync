using MediatR;

namespace MentorSync.SharedKernel.BaseEntities;

public class DomainEvent : INotification
{
    public DateTime DateOccurred { get; init; } = DateTime.UtcNow;
}
