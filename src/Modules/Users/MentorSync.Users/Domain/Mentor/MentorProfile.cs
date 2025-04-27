using System.ComponentModel.DataAnnotations;
using MentorSync.Users.Domain.Enums;
using MentorSync.Users.Domain.User;

namespace MentorSync.Users.Domain.Mentor;

public sealed class MentorProfile
{
    public int Id { get; set; }
    public Industry Industries { get; set; }
    [Length(1, 20)]
    public List<string> Skills { get; set; }
    [Length(1, 10)]
    public List<string> ProgrammingLanguages { get; set; }
    public int ExperienceYears { get; set; }
    public Availability Availability { get; set; }

    public int MentorId { get; set; }
    public AppUser User { get; set; }
}
