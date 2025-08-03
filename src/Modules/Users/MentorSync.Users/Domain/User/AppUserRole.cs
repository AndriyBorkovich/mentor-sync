using MentorSync.Users.Domain.Role;
using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain.User;

public sealed class AppUserRole : IdentityUserRole<int>
{
	public AppUser User { get; set; }
	public AppRole Role { get; set; }
}
