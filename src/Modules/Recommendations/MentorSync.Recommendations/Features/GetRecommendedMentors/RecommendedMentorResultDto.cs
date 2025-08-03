using MentorSync.SharedKernel.CommonEntities.Enums;

namespace MentorSync.Recommendations.Features.GetRecommendedMentors;

/// <summary>
/// DTO for SQL query results with recommendations data
/// </summary>
public sealed class RecommendedMentorResultDto
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Title { get; set; }
	public double Rating { get; set; }
	public string[] Skills { get; set; }
	public string ProfileImage { get; set; }
	public int? ExperienceYears { get; set; }
	public string[] ProgrammingLanguages { get; set; }
	public Industry Industries { get; set; }
	public bool IsActive { get; set; }
	public float CollaborativeScore { get; set; }
	public float ContentBasedScore { get; set; }
	public float FinalScore { get; set; }
}
