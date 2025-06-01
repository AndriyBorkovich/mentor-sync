namespace MentorSync.Recommendations.Features.Pipelines.Base;

/// <summary>
/// trains and saves the ML.NET model
/// </summary>
public interface ICollaborativeTrainer
{
    Task TrainAsync(CancellationToken cancellationToken);
}

