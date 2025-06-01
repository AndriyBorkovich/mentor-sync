namespace MentorSync.Recommendations.Features.Pipelines.Base;

/// <summary>
/// collects and scores events (ETL)
/// </summary>
public interface IInteractionAggregator
{
    Task RunAsync(CancellationToken cancellationToken);
}


