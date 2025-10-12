using System.ComponentModel.DataAnnotations;
using MentorSync.Users.Domain.Base;
using MentorSync.Users.Domain.Enums;
using MentorSync.Users.Domain.User;

namespace MentorSync.Users.Domain.Mentor;

/// <summary>
/// Represents a mentor's profile, including experience and availability.
/// </summary>
public sealed class MentorProfile : BaseProfile
{
	/// <summary>
	/// The number of years of experience the mentor has.
	/// </summary>
	[Range(0, 50, ErrorMessage = "Experience must be between 0 and 50 years.")]
	public int ExperienceYears { get; set; }
	/// <summary>
	/// The areas of expertise the mentor specializes in.
	/// </summary>
	public Availability Availability { get; set; }

	/// <summary>
	/// The unique identifier of the mentor (user).
	/// </summary>
	public int MentorId { get; set; }
	/// <summary>
	/// The user associated with this mentor profile.
	/// </summary>
	public AppUser User { get; set; }
}
