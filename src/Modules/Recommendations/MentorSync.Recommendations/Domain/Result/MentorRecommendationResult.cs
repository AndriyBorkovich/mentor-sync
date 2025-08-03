namespace MentorSync.Recommendations.Domain.Result;

/// <summary>
/// Stores combined CF+CBF scores and final mentor recommendations per mentee.
/// </summary>
public sealed class MentorRecommendationResult : BaseRecommendationResult
{
	public int MentorId { get; set; }
}
