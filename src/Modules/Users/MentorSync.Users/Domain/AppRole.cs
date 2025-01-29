using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain;

public class AppRole : IdentityRole<int>
{
    public virtual ICollection<AppUserRole> UserRoles { get; set; }
    public virtual ICollection<AppRoleClaim> RoleClaims { get; set; }
}