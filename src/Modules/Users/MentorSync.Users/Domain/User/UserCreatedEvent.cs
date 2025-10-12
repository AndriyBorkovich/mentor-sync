using MentorSync.SharedKernel.Abstractions.DomainEvents;

namespace MentorSync.Users.Domain.User;

/// <summary>
/// Event triggered when a new user is created
/// </summary>
/// <param name="userId">User identifier</param>
public sealed class UserCreatedEvent(int userId) : DomainEvent
{
	/// <summary>
	/// Identifier of the newly created user
	/// </summary>
	public int UserId { get; set; } = userId;
}
