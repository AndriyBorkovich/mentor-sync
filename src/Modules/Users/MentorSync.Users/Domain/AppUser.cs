using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain;

public class AppUser : IdentityUser<int>
{
    [StringLength(500)]
    public string ProfileImageUrl { get; set; }
    public bool IsActive { get; set; }
    
    public virtual ICollection<AppUserClaim> Claims { get; set; }
    public virtual ICollection<AppUserLogin> Logins { get; set; }
    public virtual ICollection<AppUserToken> Tokens { get; set; }
    public virtual ICollection<AppUserRole> UserRoles { get; set; }
}