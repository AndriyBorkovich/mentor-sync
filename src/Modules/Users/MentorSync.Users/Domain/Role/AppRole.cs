using MentorSync.Users.Domain.User;
using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain.Role;

/// <summary>
/// Application role entity
/// </summary>
public sealed class AppRole : IdentityRole<int>
{
	/// <summary>
	/// Navigation property for the users in this role
	/// </summary>
	public ICollection<AppUserRole> UserRoles { get; set; }
	/// <summary>
	/// Navigation property for the claims in this role
	/// </summary>
	public ICollection<AppRoleClaim> RoleClaims { get; set; }
}
