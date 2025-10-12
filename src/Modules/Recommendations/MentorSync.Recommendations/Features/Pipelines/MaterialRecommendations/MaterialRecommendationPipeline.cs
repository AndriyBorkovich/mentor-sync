using MentorSync.Recommendations.Features.Pipelines.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MentorSync.Recommendations.Features.Pipelines.MaterialRecommendations;

/// <summary>
/// Automates the full hybrid (CF + CBF) recommendation pipeline for learning materials
/// </summary>
public sealed class MaterialRecommendationPipeline(
	IServiceProvider serviceProvider,
	ILogger<MaterialRecommendationPipeline> logger)
	: BackgroundService
{
	/// <inheritdoc />
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation("Learning material recommendation pipeline started.");

		while (!stoppingToken.IsCancellationRequested)
		{
			await using var scope = serviceProvider.CreateAsyncScope();

			try
			{
				var etl = scope.ServiceProvider.GetRequiredKeyedService<IInteractionAggregator>(ServicesConstants.MaterialsKey);
				var trainer = scope.ServiceProvider.GetRequiredKeyedService<ICollaborativeTrainer>(ServicesConstants.MaterialsKey);
				var scorer = scope.ServiceProvider.GetRequiredKeyedService<IHybridScorer>(ServicesConstants.MaterialsKey);

				await etl.RunAsync(stoppingToken);
				await trainer.TrainAsync(stoppingToken);
				await scorer.GenerateRecommendationsAsync(stoppingToken);

				logger.LogInformation("Learning material recommendation pipeline completed successfully");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Learning material recommendation pipeline failed");
			}

			await Task.Delay(TimeSpan.FromMinutes(6), stoppingToken);
		}
	}
}
