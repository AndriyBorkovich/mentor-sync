using MentorSync.Users.Domain.Role;
using MentorSync.Users.Domain.User;
using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain;

public sealed class AppUserRole : IdentityUserRole<int>
{
    public AppUser User { get; set; }
    public AppRole Role { get; set; }
}