namespace MentorSync.Recommendations.Infrastructure.MachineLearning.Output;

/// <summary>
/// Prediction result for material recommendation
/// </summary>
public sealed class MaterialPrediction
{
	/// <summary>
	/// Score of the prediction (0 to 1)
	/// </summary>
	public float Score { get; set; }
}
