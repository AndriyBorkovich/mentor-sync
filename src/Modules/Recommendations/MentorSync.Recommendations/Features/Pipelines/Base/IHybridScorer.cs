namespace MentorSync.Recommendations.Features.Pipelines.Base;

/// <summary>
/// loads model, predicts, computes CBF, and stores final scores
/// </summary>
public interface IHybridScorer
{
    Task GenerateRecommendationsAsync(CancellationToken cancellationToken);
}
