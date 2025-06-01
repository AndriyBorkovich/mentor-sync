using MentorSync.Recommendations.Features.Pipelines.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MentorSync.Recommendations.Features.Pipelines.MentorRecommendations;

/// <summary>
/// Automates the full hybrid (CF + CBF) recommendation pipeline
/// </summary>
public sealed class MentorRecommendationPipeline(
    IServiceProvider serviceProvider, ILogger<MentorRecommendationPipeline> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Mentor recommendation pipeline started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();

            try
            {
                var etl = scope.ServiceProvider.GetRequiredKeyedService<IInteractionAggregator>(ServicesConstants.MentorsKey);
                var trainer = scope.ServiceProvider.GetRequiredKeyedService<ICollaborativeTrainer>(ServicesConstants.MentorsKey);
                var scorer = scope.ServiceProvider.GetRequiredKeyedService<IHybridScorer>(ServicesConstants.MentorsKey);

                await etl.RunAsync(stoppingToken);
                await trainer.TrainAsync(stoppingToken);
                await scorer.GenerateRecommendationsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Mentor recommendation pipeline failed");
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
