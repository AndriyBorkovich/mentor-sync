using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain.User;

/// <summary>
/// Represents a user login for external authentication providers.
/// </summary>
public sealed class AppUserLogin : IdentityUserLogin<int>
{
	/// <summary>
	/// The associated user.
	/// </summary>
	public AppUser User { get; set; }
}
