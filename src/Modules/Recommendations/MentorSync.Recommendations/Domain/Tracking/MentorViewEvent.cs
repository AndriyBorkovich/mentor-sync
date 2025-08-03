namespace MentorSync.Recommendations.Domain.Tracking;

/// <summary>
/// Tracks mentees viewing mentor profiles.
/// </summary>
public sealed class MentorViewEvent : BaseViewEvent
{
	public int MentorId { get; set; }
}
