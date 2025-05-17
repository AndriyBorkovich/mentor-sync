using System.Diagnostics;
using MentorSync.Materials.Data;
using MentorSync.Ratings.Data;
using MentorSync.Recommendations.Data;
using MentorSync.Scheduling.Data;
using MentorSync.SharedKernel;
using MentorSync.Users.Data;
using MentorSync.Users.Domain.Role;
using Microsoft.AspNetCore.Identity;
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

            await MigrateAsync<UsersDbContext>(scope.ServiceProvider, cancellationToken, postMigrationStep: SeedRolesAsync);
            await MigrateAsync<SchedulingDbContext>(scope.ServiceProvider, cancellationToken);
            await MigrateAsync<MaterialsDbContext>(scope.ServiceProvider, cancellationToken);
            await MigrateAsync<RatingsDbContext>(scope.ServiceProvider, cancellationToken);
            await MigrateAsync<RecommendationDbContext>(scope.ServiceProvider, cancellationToken);

            logger.LogInformation("Migrated database successfully.");
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task MigrateAsync<T>(
        IServiceProvider sp,
        CancellationToken cancellationToken,
        Func<Task>? postMigrationStep = null)
        where T : DbContext
    {
        var context = sp.GetRequiredService<T>();
        await EnsureDatabaseAsync(context, cancellationToken);
        await RunMigrationAsync(context, cancellationToken);
        if (postMigrationStep is not null)
            await postMigrationStep();
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

    private async Task SeedRolesAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        await CreateRole(Roles.Admin);
        await CreateRole(Roles.Mentor);
        await CreateRole(Roles.Mentee);

        return;

        async Task CreateRole(string roleName)
        {
            var appRole = await roleManager.FindByNameAsync(roleName);

            if (appRole is null)
            {
                appRole = new AppRole
                {
                    Name = roleName
                };

                await roleManager.CreateAsync(appRole);
            }
        }
    }
}
