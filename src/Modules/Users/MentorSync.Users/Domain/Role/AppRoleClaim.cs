using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain.Role;

public sealed class AppRoleClaim : IdentityRoleClaim<int>
{
    public AppRole Role { get; set; }
}
