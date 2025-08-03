using MentorSync.Materials.Data;
using MentorSync.Ratings.Data;
using MentorSync.Recommendations.Data;
using MentorSync.Scheduling.Data;
using MentorSync.SharedKernel;
using MentorSync.Users.Data;
using MentorSync.Users.Domain.Role;
using MentorSync.Users.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.MigrationService;

public static class DependencyInjectionExtensions
{
	public static void AddMockEventDispatcher(this IServiceCollection services)
	{
		services.AddSingleton<IDomainEventsDispatcher, EmptyDomainEventsDispatcher>();
	}

	public static void AddDbContexts(this IHostApplicationBuilder builder)
	{
		AddDbContext<UsersDbContext>(SchemaConstants.Users);
		AddIdentity();
		AddDbContext<SchedulingDbContext>(SchemaConstants.Scheduling);
		AddDbContext<MaterialsDbContext>(SchemaConstants.Materials);
		AddDbContext<RatingsDbContext>(SchemaConstants.Ratings);
		AddDbContext<RecommendationsDbContext>(SchemaConstants.Recommendations);

		void AddDbContext<T>(string schemaName)
			where T : DbContext
			=> builder.AddNpgsqlDbContext<T>(
				connectionName: GeneralConstants.DatabaseName,
				configureSettings: c => c.DisableTracing = true,
				configureDbContextOptions: opt
					=> opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, schemaName)));

		void AddIdentity()
			=> builder.Services.AddIdentity<AppUser, AppRole>()
					.AddEntityFrameworkStores<UsersDbContext>()
					.AddDefaultTokenProviders();
	}

	public static void AddMigrationsWorker(this IServiceCollection services)
	{
		services.AddHostedService<Worker>();

		services.AddOpenTelemetry()
			.WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));
	}
}
