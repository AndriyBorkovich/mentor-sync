using System.ComponentModel.DataAnnotations;
using MentorSync.Users.Domain.Base;
using MentorSync.Users.Domain.User;

namespace MentorSync.Users.Domain.Mentee;

/// <summary>
/// Represents a mentee profile in the system
/// </summary>
public sealed class MenteeProfile : BaseProfile
{
	/// <summary>
	/// The learning goals of the mentee
	/// </summary>
	[Length(1, 20)]
	public ICollection<string> LearningGoals { get; set; }

	/// <summary>
	/// The ID of the associated user
	/// </summary>
	public int MenteeId { get; set; }
	/// <summary>
	/// The associated user
	/// </summary>
	public AppUser User { get; set; }
}
