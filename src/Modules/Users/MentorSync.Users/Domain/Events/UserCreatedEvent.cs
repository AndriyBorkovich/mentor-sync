using MentorSync.SharedKernel.BaseEntities;

namespace MentorSync.Users.Domain.Events;

public class UserCreatedEvent(int userId) : DomainEvent
{
    public int UserId { get; set; } = userId;
}
