using MentorSync.SharedKernel.BaseEntities;

namespace MentorSync.Users.Domain.User;

public class UserCreatedEvent(int userId) : DomainEvent
{
    public int UserId { get; set; } = userId;
}
