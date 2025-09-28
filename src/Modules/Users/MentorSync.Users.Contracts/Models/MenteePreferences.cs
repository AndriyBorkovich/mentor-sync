namespace MentorSync.Users.Contracts.Models;

/// <summary>
/// Represents mentee preferences for finding suitable mentors
/// </summary>
public sealed class MenteePreferences
{
	/// <summary>
	/// Gets or sets the mentee's unique identifier
	/// </summary>
	public int MenteeId { get; set; }

	/// <summary>
	/// Gets or sets the desired industries the mentee wants to work in
	/// </summary>
	public Industry DesiredIndustries { get; set; } = default!;

	/// <summary>
	/// Gets or sets the list of programming languages the mentee wants to learn
	/// </summary>
	public List<string> DesiredProgrammingLanguages { get; set; } = default!;

	/// <summary>
	/// Gets or sets the skills the mentee wants to develop
	/// </summary>
	public List<string> DesiredSkills { get; set; } = default!;

	/// <summary>
	/// Gets or sets the position the mentee is targeting
	/// </summary>
	public string Position { get; set; } = default!;

	/// <summary>
	/// Gets or sets the minimum years of experience required from potential mentors
	/// </summary>
	public int? MinMentorExperienceYears { get; set; }
}
