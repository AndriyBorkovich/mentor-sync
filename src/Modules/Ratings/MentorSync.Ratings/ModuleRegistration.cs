using FluentValidation;
using MentorSync.Ratings.Contracts.Services;
using MentorSync.Ratings.Data;
using MentorSync.Ratings.Services;
using MentorSync.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentorSync.Ratings;

public static class ModuleRegistration
{
	public static void AddRatingsModule(this IHostApplicationBuilder builder)
	{
		AddDatabase(builder);

		AddEndpoints(builder.Services);

		AddExternalServices(builder.Services);
	}

	private static void AddDatabase(IHostApplicationBuilder builder)
		=> builder.AddNpgsqlDbContext<RatingsDbContext>(
				connectionName: GeneralConstants.DatabaseName,
				configureSettings: c => c.DisableTracing = true,
				configureDbContextOptions: opt
					=> opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Ratings)));

	private static void AddEndpoints(IServiceCollection services)
	{
		var assembly = typeof(ModuleRegistration).Assembly;
		services.AddValidatorsFromAssembly(assembly);
		services.AddHandlers(assembly);
		services.AddEndpoints(assembly);
	}

	private static void AddExternalServices(IServiceCollection services)
	{
		services.AddScoped<IMentorReviewService, MentorReviewService>();
		services.AddScoped<IMaterialReviewService, MaterialReviewService>();
	}
}
