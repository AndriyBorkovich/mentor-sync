using System.ComponentModel.DataAnnotations;

namespace MentorSync.Users.Domain.Base;

/// <summary>
/// Base profile class containing common profile properties
/// </summary>
public class BaseProfile
{
	/// <summary>
	/// Primary key for the profile
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	/// Biography of the user
	/// </summary>
	[StringLength(2000)]
	public required string Bio { get; set; }
	/// <summary>
	/// Current position of the user
	/// </summary>
	[StringLength(100)]
	public required string Position { get; set; }
	/// <summary>
	/// Company of the user
	/// </summary>
	[StringLength(100)]
	public string Company { get; set; }
	/// <summary>
	/// Industries the user is associated with
	/// </summary>
	public required Industry Industries { get; set; }
	/// <summary>
	/// Skills of the user
	/// </summary>
	[Length(1, 20)]
	public ICollection<string> Skills { get; set; }
	/// <summary>
	/// Programming languages known by the user
	/// </summary>
	[Length(1, 10)]
	public ICollection<string> ProgrammingLanguages { get; set; }
}
