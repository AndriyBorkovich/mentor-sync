using MentorSync.SharedKernel.CommonEntities;

namespace MentorSync.Users.Contracts.Models;

public sealed class MentorProfileModel
{
    public int Id { get; set; }
    public IEnumerable<string> ProgrammingLanguages { get; set; }
    public int? ExperienceYears { get; set; }
    public string CommunicationLanguage { get; set; }
    public Industry Industry { get; set; }
}
