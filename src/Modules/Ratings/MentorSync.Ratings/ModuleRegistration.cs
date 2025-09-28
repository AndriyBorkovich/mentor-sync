using FluentValidation;
using MentorSync.Ratings.Contracts.Services;
using MentorSync.Ratings.Data;
using MentorSync.Ratings.Services;
using MentorSync.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentorSync.Ratings;

/// <summary>
/// Registration module for Ratings domain services and dependencies
/// </summary>
public static class ModuleRegistration
{
	/// <summary>
	/// Registers all Ratings module services including database context, endpoints, and external services
	/// </summary>
	/// <param name="builder">The host application builder to configure</param>
	public static void AddRatingsModule(this IHostApplicationBuilder builder)
	{
		AddDatabase(builder);

		AddEndpoints(builder.Services);

		AddExternalServices(builder.Services);
	}

	/// <summary>
	/// Configures the PostgreSQL database context for the Ratings module
	/// </summary>
	/// <param name="builder">The host application builder</param>
	private static void AddDatabase(IHostApplicationBuilder builder)
		=> builder.AddNpgsqlDbContext<RatingsDbContext>(
				connectionName: GeneralConstants.DatabaseName,
				configureSettings: c => c.DisableTracing = true,
				configureDbContextOptions: opt
					=> opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Ratings)));

	/// <summary>
	/// Registers endpoints, validators, and handlers for the Ratings module
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
	/// Registers external services exposed by the Ratings module
	/// </summary>
	/// <param name="services">The service collection to configure</param>
	private static void AddExternalServices(IServiceCollection services)
	{
		services.AddScoped<IMentorReviewService, MentorReviewService>();
		services.AddScoped<IMaterialReviewService, MaterialReviewService>();
	}
}
