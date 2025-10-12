namespace MentorSync.Users.Features.SearchMentors;

/// <summary>
/// Data Transfer Object representing a mentor in search results
/// </summary>
public sealed class MentorSearchResultDto
{
	/// <summary>
	/// Unique identifier for the mentor
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	/// Full name of the mentor
	/// </summary>
	public string Name { get; set; }
	/// <summary>
	/// Professional title of the mentor
	/// </summary>
	public string Title { get; set; }
	/// <summary>
	/// Average rating of the mentor
	/// </summary>
	public double Rating { get; set; }
	/// <summary>
	/// Number of reviews the mentor has received
	/// </summary>
	public string[] Skills { get; set; }
	/// <summary>
	/// URL to the mentor's profile image
	/// </summary>
	public string ProfileImage { get; set; }
	/// <summary>
	/// Years of professional experience the mentor has
	/// </summary>
	public int? ExperienceYears { get; set; }
	/// <summary>
	/// Industry category the mentor specializes in
	/// </summary>
	public Industry Industries { get; set; }
	/// <summary>
	/// Programming languages the mentor is proficient in
	/// </summary>
	public string[] ProgrammingLanguages { get; set; }
	/// <summary>
	/// Indicates whether the mentor is currently active
	/// </summary>
	public bool IsActive { get; set; }
}

