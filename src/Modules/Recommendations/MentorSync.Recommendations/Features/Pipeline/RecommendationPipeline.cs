using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MentorSync.Recommendations.Features.Pipeline;

/// <summary>
/// Automates the full hybrid (CF + CBF) recommendation pipeline
/// </summary>
public sealed class RecommendationPipeline(
    IServiceProvider serviceProvider, ILogger<RecommendationPipeline> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Recommendation pipeline started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();

            try
            {
                var etl = scope.ServiceProvider.GetRequiredService<IInteractionAggregator>();
                var trainer = scope.ServiceProvider.GetRequiredService<ICollaborativeTrainer>();
                var scorer = scope.ServiceProvider.GetRequiredService<IHybridScorer>();

                await etl.RunAsync(stoppingToken);
                await trainer.TrainAsync(stoppingToken);
                await scorer.GenerateRecommendationsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Recommendation pipeline failed");
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // adjust frequency
        }
    }
}
