using MentorSync.SharedKernel.CommonEntities.Enums;

namespace MentorSync.Users.Features.SearchMentors;

public sealed class MentorSearchResultDto
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Title { get; set; }
	public double Rating { get; set; }
	public string[] Skills { get; set; }
	public string ProfileImage { get; set; }
	public int? ExperienceYears { get; set; }
	public Industry Industries { get; set; }
	public string[] ProgrammingLanguages { get; set; }
	public bool IsActive { get; set; }
}

