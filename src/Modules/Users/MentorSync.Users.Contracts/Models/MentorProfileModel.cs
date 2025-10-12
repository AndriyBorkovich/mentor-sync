namespace MentorSync.Users.Contracts.Models;

/// <summary>
/// Represents a mentor profile model for external module access
/// </summary>
public sealed class MentorProfileModel
{
	/// <summary>
	/// Gets or sets the mentor's unique identifier
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the mentor's username
	/// </summary>
	public string UserName { get; set; }

	/// <summary>
	/// Gets or sets the programming languages the mentor knows
	/// </summary>
	public IEnumerable<string> ProgrammingLanguages { get; set; }

	/// <summary>
	/// Gets or sets the mentor's years of professional experience
	/// </summary>
	public int? ExperienceYears { get; set; }

	/// <summary>
	/// Gets or sets the industry the mentor works in
	/// </summary>
	public Industry Industry { get; set; }

	/// <summary>
	/// Gets or sets the mentor's current position/role
	/// </summary>
	public string Position { get; set; }

	/// <summary>
	/// Gets or sets the list of skills the mentor possesses
	/// </summary>
	public IReadOnlyList<string> Skills { get; set; }
}
