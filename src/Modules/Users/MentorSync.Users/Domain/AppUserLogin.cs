using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain;

public sealed class AppUserLogin : IdentityUserLogin<int>
{
    public AppUser User { get; set; }
}