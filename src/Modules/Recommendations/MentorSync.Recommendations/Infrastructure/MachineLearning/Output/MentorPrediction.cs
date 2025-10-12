namespace MentorSync.Recommendations.Infrastructure.MachineLearning.Output;

/// <summary>
/// Prediction result for mentor recommendations
/// </summary>
public sealed class MentorPrediction
{
	/// <summary>
	/// The predicted score for the mentor
	/// </summary>
	public float Score { get; set; }
}
