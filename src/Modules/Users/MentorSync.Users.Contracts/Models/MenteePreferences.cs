using MentorSync.SharedKernel.CommonEntities;

namespace MentorSync.Users.Contracts.Models;

public sealed class MenteePreferences
{
    public int MenteeId { get; set; }
    public Industry DesiredIndustries { get; set; } = default!;
    public List<string> DesiredProgrammingLanguages { get; set; } = default!;
    public int? MinMentorExperienceYears { get; set; }
}
