namespace MentorSync.Recommendations.Features.GetRecommendedMentors;

/// <summary>
/// DTO for SQL query results with recommendations data
/// </summary>
public sealed class RecommendedMentorResultDto
{
	/// <summary>
	/// Gets or sets the mentor's unique identifier
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the mentor's full name
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// Gets or sets the mentor's professional title
	/// </summary>
	public string Title { get; set; }

	/// <summary>
	/// Gets or sets the mentor's overall rating (0-5)
	/// </summary>
	public double Rating { get; set; }

	/// <summary>
	/// Gets or sets the mentor's skill set
	/// </summary>
	public string[] Skills { get; set; }

	/// <summary>
	/// Gets or sets the mentor's profile image URL
	/// </summary>
	public string ProfileImage { get; set; }

	/// <summary>
	/// Gets or sets the mentor's years of experience
	/// </summary>
	public int? ExperienceYears { get; set; }

	/// <summary>
	/// Gets or sets the programming languages the mentor knows
	/// </summary>
	public string[] ProgrammingLanguages { get; set; }

	/// <summary>
	/// Gets or sets the mentor's industry
	/// </summary>
	public Industry Industries { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the mentor is currently active
	/// </summary>
	public bool IsActive { get; set; }

	/// <summary>
	/// Gets or sets the collaborative filtering score for this recommendation
	/// </summary>
	public float CollaborativeScore { get; set; }

	/// <summary>
	/// Gets or sets the content-based filtering score for this recommendation
	/// </summary>
	public float ContentBasedScore { get; set; }

	/// <summary>
	/// Gets or sets the final combined recommendation score
	/// </summary>
	public float FinalScore { get; set; }
}
