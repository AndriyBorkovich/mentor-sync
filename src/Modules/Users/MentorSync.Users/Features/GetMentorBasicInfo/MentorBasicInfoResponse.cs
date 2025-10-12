namespace MentorSync.Users.Features.GetMentorBasicInfo;

/// <summary>
/// Response model for basic mentor information
/// </summary>
public sealed record MentorBasicInfoResponse
{
	/// <summary>
	/// Unique identifier of the mentor
	/// </summary>
	public int Id { get; init; }
	/// <summary>
	/// Full name of the mentor
	/// </summary>
	public string Name { get; init; }
	/// <summary>
	/// Professional title of the mentor
	/// </summary>
	public string Title { get; init; }
	/// <summary>
	/// Average rating of the mentor
	/// </summary>
	public double Rating { get; init; }
	/// <summary>
	/// Number of reviews the mentor has received
	/// </summary>
	public int ReviewsCount { get; init; }
	/// <summary>
	/// URL to the mentor's profile image
	/// </summary>
	public string ProfileImage { get; init; }
	/// <summary>
	/// Years of professional experience the mentor has
	/// </summary>
	public int? YearsOfExperience { get; init; }
	/// <summary>
	/// Industry category the mentor specializes in
	/// </summary>
	public string Category { get; init; }
	/// <summary>
	/// Short biography of the mentor
	/// </summary>
	public string Bio { get; init; }
	/// <summary>
	/// Availability status of the mentor (e.g., "Available", "Busy", "On Vacation")
	/// </summary>
	public string Availability { get; init; }
	/// <summary>
	/// List of programming languages the mentor is proficient in
	/// </summary>
	public IReadOnlyList<string> ProgrammingLanguages { get; init; } = [];
	/// <summary>
	/// List of skills the mentor possesses
	/// </summary>
	public IReadOnlyList<MentorSkillDto> Skills { get; init; } = [];
}

/// <summary>
/// Data transfer object representing a mentor's skill
/// </summary>
public sealed record MentorSkillDto
{
	/// <summary>
	/// Unique identifier of the skill
	/// </summary>
	public string Id { get; init; }
	/// <summary>
	/// Name of the skill
	/// </summary>
	public string Name { get; init; }
}
