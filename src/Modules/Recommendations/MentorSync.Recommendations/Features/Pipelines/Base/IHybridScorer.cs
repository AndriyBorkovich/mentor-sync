namespace MentorSync.Recommendations.Features.Pipelines.Base;

/// <summary>
/// loads model, predicts, computes CBF, and stores final scores
/// </summary>
public interface IHybridScorer
{
	/// <summary>
	/// Generates hybrid recommendations by combining collaborative and content-based filtering scores
	/// </summary>
	/// <param name="cancellationToken">Token to cancel a task</param>
	/// <returns>Task</returns>
	Task GenerateRecommendationsAsync(CancellationToken cancellationToken);
}
