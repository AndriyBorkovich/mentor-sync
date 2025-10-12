using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain.User;

/// <summary>
/// Represents an authentication token for a user.
/// </summary>
public sealed class AppUserToken : IdentityUserToken<int>
{
	/// <summary>
	/// The user associated with this token.
	/// </summary>
	public AppUser User { get; set; }
}
