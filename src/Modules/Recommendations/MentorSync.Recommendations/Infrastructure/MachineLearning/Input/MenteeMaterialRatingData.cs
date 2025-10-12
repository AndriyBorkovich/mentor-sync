using Microsoft.ML.Data;

namespace MentorSync.Recommendations.Infrastructure.MachineLearning.Input;

/// <summary>
/// Input data schema for mentee-material interaction ratings
/// </summary>
public sealed class MenteeMaterialRatingData
{
	/// <summary>
	/// Mentee identifier
	/// </summary>
	[LoadColumn(0)]
	public string MenteeId { get; set; } = null!;

	/// <summary>
	/// Material identifier
	/// </summary>
	[LoadColumn(1)]
	public string MaterialId { get; set; } = null!;

	/// <summary>
	/// Interaction score (e.g., rating or engagement level)
	/// </summary>
	[LoadColumn(2)]
	public float Label { get; set; }
}
