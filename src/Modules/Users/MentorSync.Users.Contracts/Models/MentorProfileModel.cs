namespace MentorSync.Users.Contracts.Models
{
    public sealed class MentorProfileModel
    {
        public int Id { get; set; }
        public IEnumerable<string> ProgrammingLanguages { get; set; }
        public int? ExperienceYears { get; set; }
        public object Region { get; set; }
        public string CommunicationLanguage { get; set; }
        public string Industry { get; set; }
    }
}
