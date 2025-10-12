namespace MentorSync.Recommendations.Domain.Result;

/// <summary>
/// Stores combined CF+CBF scores and final mentor recommendations per mentee.
/// </summary>
public sealed class MentorRecommendationResult : BaseRecommendationResult
{
	/// <summary>
	/// Gets or sets the mentee identifier.
	/// </summary>
	public int MentorId { get; set; }
}
