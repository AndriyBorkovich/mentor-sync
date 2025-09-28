using MentorSync.Materials.Contracts.Services;
using MentorSync.Materials.Data;
using MentorSync.Materials.Services;
using MentorSync.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentorSync.Materials;

/// <summary>
/// Registration module for Materials domain services and dependencies
/// </summary>
public static class ModuleRegistration
{
	/// <summary>
	/// Registers all Materials module services, database context, endpoints, and external services
	/// </summary>
	/// <param name="builder">The host application builder to configure</param>
	public static void AddMaterialsModule(this IHostApplicationBuilder builder)
	{
		AddDatabase(builder);

		AddEndpoints(builder.Services);

		AddExternalServices(builder.Services);
	}

	/// <summary>
	/// Configures the PostgreSQL database context for the Materials module
	/// </summary>
	/// <param name="builder">The host application builder</param>
	private static void AddDatabase(IHostApplicationBuilder builder)
		=> builder.AddNpgsqlDbContext<MaterialsDbContext>(
				connectionName: GeneralConstants.DatabaseName,
				configureSettings: c => c.DisableTracing = true,
				configureDbContextOptions: opt =>
				{
					opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Materials));
				});

	/// <summary>
	/// Registers endpoints, validators, and handlers for the Materials module
	/// </summary>
	/// <param name="services">The service collection to configure</param>
	private static void AddEndpoints(IServiceCollection services)
	{
		var assembly = typeof(ModuleRegistration).Assembly;
		services.AddValidators(assembly);
		services.AddHandlers(assembly);
		services.AddEndpoints(assembly);
	}

	/// <summary>
	/// Registers external services exposed by the Materials module
	/// </summary>
	/// <param name="services">The service collection to configure</param>
	private static void AddExternalServices(IServiceCollection services)
	{
		services.AddScoped<ILearningMaterialsService, LearningMaterialsService>();
	}
}
