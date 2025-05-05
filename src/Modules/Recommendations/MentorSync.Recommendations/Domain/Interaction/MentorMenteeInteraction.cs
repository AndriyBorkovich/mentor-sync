namespace MentorSync.Recommendations.Domain.Interaction;

/// <summary>
/// Used to store the interaction between a mentor and a mentee for collaborative filtering.
/// </summary>
public sealed class MentorMenteeInteraction
{
    public int Id { get; set; }
    public int MenteeId { get; set; }
    public int MentorId { get; set; }
    /// <summary>
    /// Aggregated score from events (views, bookmarks, likes, etc.) between a mentor and a mentee.
    /// </summary>
    public float Score { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
