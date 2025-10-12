using MentorSync.SharedKernel.Abstractions.DomainEvents;

namespace MentorSync.Users.Domain.User;

/// <summary>
/// Domain event representing a change in a user's active status
/// </summary>
/// <param name="email">User's email</param>
/// <param name="isActive">Indicator if user is active</param>
public sealed class UserActiveStatusChangedEvent(string email, bool isActive) : DomainEvent
{
	/// <summary>
	/// Gets the email of the user whose active status has changed
	/// </summary>
	public string Email { get; } = email;

	/// <summary>
	/// Gets a value indicating whether the user is now active
	/// </summary>
	public bool IsActive { get; } = isActive;
}
