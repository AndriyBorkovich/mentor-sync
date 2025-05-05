using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MentorSync.Recommendations.Features.Pipeline;

/// <summary>
/// Automates the full hybrid (CF + CBF) recommendation pipeline
/// </summary>
public sealed class RecommendationPipelineService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RecommendationPipelineService> _logger;

    public RecommendationPipelineService(IServiceProvider serviceProvider, ILogger<RecommendationPipelineService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Recommendation pipeline started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();

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
                _logger.LogError(ex, "Recommendation pipeline failed");
            }

            await Task.Delay(TimeSpan.FromHours(6), stoppingToken); // adjust frequency
        }
    }
}
