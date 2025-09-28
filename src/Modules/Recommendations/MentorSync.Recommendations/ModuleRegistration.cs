using FluentValidation;
using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Features.Pipelines.Base;
using MentorSync.Recommendations.Features.Pipelines.MaterialRecommendations;
using MentorSync.Recommendations.Features.Pipelines.MentorRecommendations;
using MentorSync.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentorSync.Recommendations;

/// <summary>
/// Registration module for Recommendations domain services and dependencies
/// </summary>
public static class ModuleRegistration
{
	/// <summary>
	/// Registers all Recommendations module services including database context, endpoints, and recommendation pipelines
	/// </summary>
	/// <param name="builder">The host application builder to configure</param>
	public static void AddRecommendationsModule(this IHostApplicationBuilder builder)
	{
		AddDatabase(builder);

		AddEndpoints(builder.Services);

		AddBackgroundJobs(builder.Services);
	}

	/// <summary>
	/// Configures the PostgreSQL database context for the Recommendations module
	/// </summary>
	/// <param name="builder">The host application builder</param>
	private static void AddDatabase(IHostApplicationBuilder builder)
		=> builder.AddNpgsqlDbContext<RecommendationsDbContext>(
				connectionName: GeneralConstants.DatabaseName,
				configureSettings: c => c.DisableTracing = true,
				configureDbContextOptions: opt =>
					opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Recommendations)));

	/// <summary>
	/// Registers endpoints, validators, and handlers for the Recommendations module
	/// </summary>
	/// <param name="services">The service collection to configure</param>
	private static void AddEndpoints(IServiceCollection services)
	{
		var assembly = typeof(ModuleRegistration).Assembly;

		services.AddValidatorsFromAssembly(assembly);
		services.AddHandlers(assembly);
		services.AddEndpoints(assembly);
	}

	/// <summary>
	/// Registers background job services for recommendation generation including collaborative filtering and hybrid scoring
	/// </summary>
	/// <param name="services">The service collection to configure</param>
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
