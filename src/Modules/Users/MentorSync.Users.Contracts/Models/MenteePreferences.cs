using MentorSync.SharedKernel.CommonEntities.Enums;

namespace MentorSync.Users.Contracts.Models;

public sealed class MenteePreferences
{
    public int MenteeId { get; set; }
    public Industry DesiredIndustries { get; set; } = default!;
    public List<string> DesiredProgrammingLanguages { get; set; } = default!;
    public List<string> DesiredSkills { get; set; } = default!;
    public string Position { get; set; } = default!;
    public int? MinMentorExperienceYears { get; set; }
}
