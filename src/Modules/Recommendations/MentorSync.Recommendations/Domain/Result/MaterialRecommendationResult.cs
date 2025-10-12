namespace MentorSync.Recommendations.Domain.Result;

/// <summary>
/// Stores combined CF+CBF scores and final learning material recommendations per mentee.
/// </summary>
public sealed class MaterialRecommendationResult : BaseRecommendationResult
{
	/// <summary>
	/// Gets or sets the identifier of the learning material.
	/// </summary>
	public int MaterialId { get; set; }
}
