using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain.User;

public sealed class AppUserClaim : IdentityUserClaim<int>
{
    public AppUser User { get; set; }
}
