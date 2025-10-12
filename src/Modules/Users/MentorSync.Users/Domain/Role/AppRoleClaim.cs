using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain.Role;

/// <summary>
/// Represents a claim that is associated with a role.
/// </summary>
public sealed class AppRoleClaim : IdentityRoleClaim<int>
{
	/// <summary>
	/// The role associated with this claim.
	/// </summary>
	public AppRole Role { get; set; }
}
