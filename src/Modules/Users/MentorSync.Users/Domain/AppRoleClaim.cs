using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain;

public class AppRoleClaim : IdentityRoleClaim<int>
{
    public virtual AppRole Role { get; set; }
}