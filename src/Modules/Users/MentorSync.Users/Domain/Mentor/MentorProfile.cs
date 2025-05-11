using System.ComponentModel.DataAnnotations;
using MentorSync.Users.Domain.Base;
using MentorSync.Users.Domain.Enums;
using MentorSync.Users.Domain.User;

namespace MentorSync.Users.Domain.Mentor;

public sealed class MentorProfile : BaseProfile
{
    [Range(0, 50, ErrorMessage = "Experience must be between 0 and 50 years.")]
    public int ExperienceYears { get; set; }
    public Availability Availability { get; set; }

    public int MentorId { get; set; }
    public AppUser User { get; set; }
}
