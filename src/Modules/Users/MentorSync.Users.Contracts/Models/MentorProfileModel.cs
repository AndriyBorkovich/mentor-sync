using MentorSync.SharedKernel.CommonEntities.Enums;

namespace MentorSync.Users.Contracts.Models;

public sealed class MentorProfileModel
{
	public int Id { get; set; }
	public string UserName { get; set; }
	public IEnumerable<string> ProgrammingLanguages { get; set; }
	public int? ExperienceYears { get; set; }
	public Industry Industry { get; set; }
	public string Position { get; set; }
	public List<string> Skills { get; set; }
}
