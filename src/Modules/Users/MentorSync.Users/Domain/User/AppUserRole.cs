using MentorSync.Users.Domain.Role;
using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain.User;

/// <summary>
/// Represents the association between a user and a role in the application. (many-to-many relationship)
/// </summary>
public sealed class AppUserRole : IdentityUserRole<int>
{
	/// <summary>
	/// The user associated with this role.
	/// </summary>
	public AppUser User { get; set; }
	/// <summary>
	/// The role associated with this user.
	/// </summary>
	public AppRole Role { get; set; }
}
