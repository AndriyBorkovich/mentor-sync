using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain;

public sealed class AppUserToken : IdentityUserToken<int>
{
    public AppUser User { get; set; }
}