using MentorSync.SharedKernel.Abstractions.DomainEvents;

namespace MentorSync.Users.Domain.User;

public sealed class UserActiveStatusChageEvent(string email, bool isActive) : DomainEvent
{
	public string Email { get; } = email;
	public bool IsActive { get; } = isActive;
}
