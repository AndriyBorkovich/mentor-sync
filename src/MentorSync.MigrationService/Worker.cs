using System.Diagnostics;
using MentorSync.Materials.Data;
using MentorSync.MigrationService.Seeders;
using MentorSync.Ratings.Data;
using MentorSync.Recommendations.Data;
using MentorSync.Scheduling.Data;
using MentorSync.Users.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace MentorSync.MigrationService;

/// <summary>
/// Background service that handles database migrations and seeding for all modules
/// </summary>
/// <param name="serviceProvider">Service provider for accessing database contexts</param>
/// <param name="logger">Logger for migration activities</param>
/// <param name="hostApplicationLifetime">Host application lifetime for stopping the service</param>
public sealed class Worker(
	IServiceProvider serviceProvider,
	ILogger<Worker> logger,
	IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
	/// <summary>
	/// Activity source name for tracing migration operations
	/// </summary>
	public const string ActivitySourceName = "Migrations";

	/// <summary>
	/// Activity source for tracing migration operations
	/// </summary>
	private static readonly ActivitySource _activitySource = new(ActivitySourceName);

	/// <summary>
	/// Executes the migration and seeding process for all database contexts
	/// </summary>
	/// <param name="stoppingToken">Cancellation token for stopping the operation</param>
	/// <returns>A task representing the asynchronous operation</returns>
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using var activity = _activitySource.StartActivity(ActivityKind.Client);

		try
		{
			await using var scope = serviceProvider.CreateAsyncScope();

			// CleanDatabase(scope.ServiceProvider);

			await MigrateAsync<UsersDbContext>(
				scope.ServiceProvider,
				stoppingToken,
				() => RolesSeeder.SeedAsync(scope.ServiceProvider),
				() => MentorsSeeder.SeedAsync(scope.ServiceProvider, logger),
				() => MenteesSeeder.SeedAsync(scope.ServiceProvider, logger));

			await MigrateAsync<SchedulingDbContext>(
				scope.ServiceProvider,
				stoppingToken,
				postMigrationSteps:
					() => MentorAvailabilitySeeder.SeedAsync(scope.ServiceProvider, logger));

			await MigrateAsync<RecommendationsDbContext>(scope.ServiceProvider, stoppingToken);

			await MigrateAsync<RatingsDbContext>(
			  scope.ServiceProvider,
			  stoppingToken,
			  () => MentorReviewsSeeder.SeedAsync(scope.ServiceProvider, logger));

			await MigrateAsync<MaterialsDbContext>(
				scope.ServiceProvider,
				stoppingToken,
				postMigrationSteps:
					() => LearningMaterialsSeeder.SeedAsync(scope.ServiceProvider, logger));

			logger.LogInformation("Migrated database successfully.");
		}
		catch (Exception ex)
		{
			activity?.AddException(ex);
			throw;
		}

		hostApplicationLifetime.StopApplication();
	}

	/// <summary>
	/// Cleans all database contexts by deleting their databases
	/// </summary>
	/// <param name="serviceProvider">Service provider for accessing database contexts</param>
	private static void CleanDatabase(IServiceProvider serviceProvider)
	{
		CleanDbContext<UsersDbContext>(serviceProvider);
		CleanDbContext<SchedulingDbContext>(serviceProvider);
		CleanDbContext<MaterialsDbContext>(serviceProvider);
		CleanDbContext<RatingsDbContext>(serviceProvider);
		CleanDbContext<RecommendationsDbContext>(serviceProvider);
	}

	/// <summary>
	/// Cleans a specific database context by deleting its database
	/// </summary>
	/// <typeparam name="T">The type of database context to clean</typeparam>
	/// <param name="serviceProvider">Service provider for accessing the database context</param>
	private static void CleanDbContext<T>(IServiceProvider serviceProvider)
		where T : DbContext
	{
		using var scope = serviceProvider.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<T>();

		context.Database.EnsureDeleted();
	}

	/// <summary>
	/// Migrates a specific database context and runs post-migration seeding steps
	/// </summary>
	/// <typeparam name="T">The type of database context to migrate</typeparam>
	/// <param name="sp">Service provider for accessing the database context</param>
	/// <param name="cancellationToken">Cancellation token for the operation</param>
	/// <param name="postMigrationSteps">Optional post-migration seeding steps to execute</param>
	/// <returns>A task representing the asynchronous migration operation</returns>
	private static async Task MigrateAsync<T>(
		IServiceProvider sp,
		CancellationToken cancellationToken,
		params Func<Task>[] postMigrationSteps)
		where T : DbContext
	{
		var context = sp.GetRequiredService<T>();
		await EnsureDatabaseAsync(context, cancellationToken);
		await RunMigrationAsync(context, cancellationToken);

		foreach (var step in postMigrationSteps.Where(s => s != null))
		{
			await step!();
		}
	}

	/// <summary>
	/// Ensures the database exists for the specified context
	/// </summary>
	/// <typeparam name="T">The type of database context</typeparam>
	/// <param name="dbContext">The database context to ensure exists</param>
	/// <param name="cancellationToken">Cancellation token for the operation</param>
	/// <returns>A task representing the asynchronous operation</returns>
	private static async Task EnsureDatabaseAsync<T>(T dbContext, CancellationToken cancellationToken)
		where T : DbContext
	{
		var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

		var strategy = dbContext.Database.CreateExecutionStrategy();
		await strategy.ExecuteAsync(async () =>
		{
			if (!await dbCreator.ExistsAsync(cancellationToken))
			{
				await dbCreator.CreateAsync(cancellationToken);
			}
		});
	}

	/// <summary>
	/// Runs database migrations for the specified context
	/// </summary>
	/// <typeparam name="T">The type of database context</typeparam>
	/// <param name="dbContext">The database context to migrate</param>
	/// <param name="cancellationToken">Cancellation token for the operation</param>
	/// <returns>A task representing the asynchronous operation</returns>
	private static async Task RunMigrationAsync<T>(T dbContext, CancellationToken cancellationToken)
		where T : DbContext
	{
		var strategy = dbContext.Database.CreateExecutionStrategy();
		await strategy.ExecuteAsync(async () => await dbContext.Database.MigrateAsync(cancellationToken)).ConfigureAwait(false);
	}
}
