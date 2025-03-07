using MediatR;

namespace MentorSync.SharedKernel.BaseEntities;

public class DomainEvent : INotification
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}
