namespace MentorSync.Recommendations.Domain.Result;

public class BaseRecommendationResult
{
	public int Id { get; set; }
	public int MenteeId { get; set; }

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
