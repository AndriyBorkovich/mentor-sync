namespace MentorSync.Recommendations.Domain;

/// <summary>
/// Stores combined CF+CBF scores and final mentor recommendations per mentee.
/// </summary>
public sealed class RecommendationResult
{
    public int Id { get; set; }
    public int MenteeId { get; set; }
    public int MentorId { get; set; }

    /// <summary>
    /// The score from the collaborative filtering algorithm.
    /// </summary>
    public float CollaborativeScore { get; set; }
    /// <summary>
    /// The score from the content-based filtering algorithm.
    /// </summary>
    public float ContentBasedScore { get; set; }
    /// <summary>
    /// The final score is a combination of the collaborative and content-based scores.
    /// </summary>
    public float FinalScore { get; set; }
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}
