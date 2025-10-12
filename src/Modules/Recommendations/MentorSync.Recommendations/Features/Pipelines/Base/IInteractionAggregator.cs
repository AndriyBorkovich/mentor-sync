namespace MentorSync.Recommendations.Features.Pipelines.Base;

/// <summary>
/// collects and scores events (ETL)
/// </summary>
public interface IInteractionAggregator
{
	/// <summary>
	/// Runs the ETL process to aggregate and score interactions
	/// </summary>
	/// <param name="cancellationToken">Token to cancel a task</param>
	/// <returns>Task</returns>
	Task RunAsync(CancellationToken cancellationToken);
}
