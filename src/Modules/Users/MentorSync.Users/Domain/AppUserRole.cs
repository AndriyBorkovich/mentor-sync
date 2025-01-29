using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain;

public class AppUserRole : IdentityUserRole<int>
{
    public virtual AppUser User { get; set; }
    public virtual AppRole Role { get; set; }
}