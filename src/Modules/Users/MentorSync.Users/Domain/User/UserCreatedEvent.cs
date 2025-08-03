using MentorSync.SharedKernel.Abstractions.DomainEvents;

namespace MentorSync.Users.Domain.User;

public sealed class UserCreatedEvent(int userId) : DomainEvent
{
	public int UserId { get; set; } = userId;
}
