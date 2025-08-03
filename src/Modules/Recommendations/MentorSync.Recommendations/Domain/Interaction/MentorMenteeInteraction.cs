namespace MentorSync.Recommendations.Domain.Interaction;

/// <summary>
/// Used to store the interaction between a mentor and a mentee for collaborative filtering.
/// </summary>
public sealed class MentorMenteeInteraction : BaseInteraction
{
	public int MentorId { get; set; }
}
