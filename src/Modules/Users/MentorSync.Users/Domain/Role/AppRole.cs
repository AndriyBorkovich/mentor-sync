using MentorSync.Users.Domain.User;
using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain.Role;

public sealed class AppRole : IdentityRole<int>
{
    public ICollection<AppUserRole> UserRoles { get; set; }
    public ICollection<AppRoleClaim> RoleClaims { get; set; }
}
