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

public sealed class Worker(
    IServiceProvider serviceProvider,
    ILogger<Worker> logger,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource _activitySource = new(ActivitySourceName);
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = _activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();

            // CleanDbContext<UsersDbContext>(scope.ServiceProvider);            

            await MigrateAsync<UsersDbContext>(
                scope.ServiceProvider,
                cancellationToken,
                postMigrationSteps:
                    [() => RolesSeeder.SeedAsync(scope.ServiceProvider),
                    () => MentorsSeeder.SeedAsync(scope.ServiceProvider, logger),
                    () => MenteesSeeder.SeedAsync(scope.ServiceProvider, logger)]);

            await MigrateAsync<SchedulingDbContext>(
                scope.ServiceProvider,
                cancellationToken,
                postMigrationSteps:
                    [() => MentorAvailabilitySeeder.SeedAsync(scope.ServiceProvider, logger)]);

            await MigrateAsync<MaterialsDbContext>(
                scope.ServiceProvider,
                cancellationToken,
                postMigrationSteps:
                    [() => LearningMaterialsSeeder.SeedAsync(scope.ServiceProvider, logger)]);

            await MigrateAsync<RatingsDbContext>(
                scope.ServiceProvider,
                cancellationToken,
                postMigrationSteps:
                    [() => MentorReviewsSeeder.SeedAsync(scope.ServiceProvider, logger)]);

            await MigrateAsync<RecommendationsDbContext>(scope.ServiceProvider, cancellationToken);

            logger.LogInformation("Migrated database successfully.");
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    public static void CleanDbContext<T>(IServiceProvider serviceProvider)
        where T : DbContext
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<T>();

        context.Database.EnsureDeleted();
    }

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

    private static async Task RunMigrationAsync<T>(T dbContext, CancellationToken cancellationToken)
        where T : DbContext
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }
}
