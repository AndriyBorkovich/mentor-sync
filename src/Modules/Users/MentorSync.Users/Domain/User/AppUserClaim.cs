using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain.User;

/// <summary>
/// Represents a claim that a user possesses.
/// </summary>
public sealed class AppUserClaim : IdentityUserClaim<int>
{
	/// <summary>
	/// The user associated with this claim.
	/// </summary>
	public AppUser User { get; set; }
}
