using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain;

public class AppUserToken : IdentityUserToken<int>
{
    public virtual AppUser User { get; set; }
}