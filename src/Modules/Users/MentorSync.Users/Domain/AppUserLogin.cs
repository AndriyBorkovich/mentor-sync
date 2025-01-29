using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain;

public class AppUserLogin : IdentityUserLogin<int>
{
    public virtual AppUser User { get; set; }
}