using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain;

public class AppUserClaim : IdentityUserClaim<int>
{
    public virtual AppUser User { get; set; }
}