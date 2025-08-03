using FluentValidation;
using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Features.Pipelines.Base;
using MentorSync.Recommendations.Features.Pipelines.MaterialRecommendations;
using MentorSync.Recommendations.Features.Pipelines.MentorRecommendations;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentorSync.Recommendations;

public static class ModuleRegistration
{
	public static void AddRecommendationsModule(this IHostApplicationBuilder builder)
	{
		AddDatabase(builder);

		AddEndpoints(builder.Services);

		AddBackgroundJobs(builder.Services);
	}

	private static void AddDatabase(IHostApplicationBuilder builder)
		=> builder.AddNpgsqlDbContext<RecommendationsDbContext>(
				connectionName: GeneralConstants.DatabaseName,
				configureSettings: c => c.DisableTracing = true,
				configureDbContextOptions: opt =>
					opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Recommendations)));

	private static void AddEndpoints(IServiceCollection services)
	{
		var assembly = typeof(ModuleRegistration).Assembly;

		services.AddValidatorsFromAssembly(assembly);
		services.AddHandlers(assembly);
		services.AddEndpoints(assembly);
	}

	private static void AddBackgroundJobs(this IServiceCollection services)
	{
		// Mentor recommendation services
		services.AddKeyedScoped<IInteractionAggregator, MentorInteractionAggregator>(ServicesConstants.MentorsKey);
		services.AddKeyedScoped<ICollaborativeTrainer, MentorCollaborativeTrainer>(ServicesConstants.MentorsKey);
		services.AddKeyedScoped<IHybridScorer, MentorHybridScorer>(ServicesConstants.MentorsKey);

		// Learning material recommendation services
		services.AddKeyedScoped<IInteractionAggregator, MaterialInteractionAggregator>(ServicesConstants.MaterialsKey);
		services.AddKeyedScoped<ICollaborativeTrainer, MaterialCollaborativeTrainer>(ServicesConstants.MaterialsKey);
		services.AddKeyedScoped<IHybridScorer, MaterialHybridScorer>(ServicesConstants.MaterialsKey);

		// recommendation pipelines
		services.AddHostedService<MentorRecommendationPipeline>();
		services.AddHostedService<MaterialRecommendationPipeline>();
	}
}
