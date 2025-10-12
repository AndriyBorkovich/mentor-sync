namespace MentorSync.Recommendations.Features.Pipelines.Base;

/// <summary>
/// trains and saves the ML.NET model
/// </summary>
public interface ICollaborativeTrainer
{
	/// <summary>
	/// Trains the collaborative filtering model and saves it to disk
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Task</returns>
	Task TrainAsync(CancellationToken cancellationToken);
}
