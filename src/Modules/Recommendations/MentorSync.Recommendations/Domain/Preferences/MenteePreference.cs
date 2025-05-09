using MentorSync.SharedKernel.CommonEntities;
namespace MentorSync.Recommendations.Domain.Preferences;

/// <summary>
/// Stores the mentee’s learning goals and preferred mentor characteristics for content-based filtering.
/// </summary>
public sealed class MenteePreference
{
    public int Id { get; set; }
    public int MenteeId { get; set; }

    public Industry DesiredIndustries { get; set; } = default!;
    public List<string> DesiredProgrammingLanguages { get; set; } = default!;
    public int? MinMentorExperienceYears { get; set; }
    public string PreferredCommunicationLanguage { get; set; } = default!;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
