using Microsoft.ML.Data;

namespace MentorSync.Recommendations.Infrastructure.MachineLearning.Input;

/// <summary>
/// Data model for mentee-mentor interaction used in ML.NET
/// </summary>
public sealed class MenteeMentorRatingData
{
	/// <summary>
	/// Mentee identifier
	/// </summary>
	[LoadColumn(0)]
	public string MenteeId { get; set; } = null!;

	/// <summary>
	/// Mentor identifier
	/// </summary>
	[LoadColumn(1)]
	public string MentorId { get; set; } = null!;

	/// <summary>
	/// Interaction score (e.g., rating or engagement level)
	/// </summary>
	[LoadColumn(2)]
	public float Label { get; set; }
}
