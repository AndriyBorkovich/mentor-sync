using Microsoft.ML.Data;

namespace MentorSync.Recommendations.Infrastructure.MachineLearning.Input;

public sealed class MenteeMentorRatingData
{
	[LoadColumn(0)]
	public string MenteeId { get; set; } = null!;

	[LoadColumn(1)]
	public string MentorId { get; set; } = null!;

	[LoadColumn(2)]
	public float Label { get; set; } // Interaction score
}
