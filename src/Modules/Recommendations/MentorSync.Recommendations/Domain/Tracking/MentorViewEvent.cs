namespace MentorSync.Recommendations.Domain.Tracking;

/// <summary>
/// Tracks mentees viewing mentor profiles.
/// </summary>
public sealed class MentorViewEvent : BaseViewEvent
{
	/// <summary>
	/// Identifier of the mentor being viewed.
	/// </summary>
	public int MentorId { get; set; }
}
