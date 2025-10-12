namespace MentorSync.Recommendations.Domain.Result;

/// <summary>
/// Base class for recommendation results
/// </summary>
public abstract class BaseRecommendationResult
{
	/// <summary>
	/// Unique identifier for the recommendation result
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	/// The identifier of the recommended mentor.
	/// </summary>
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

	/// <summary>
	/// The date and time when the recommendation was created.
	/// </summary>
	public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}
