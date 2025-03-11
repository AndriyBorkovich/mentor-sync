using System.Diagnostics;
using MentorSync.SharedKernel;
using MentorSync.Users.Data;
using MentorSync.Users.Domain;
using MentorSync.Users.Domain.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace MentorSync.MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    ILogger<Worker> logger,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var usersDbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();

            await EnsureDatabaseAsync(usersDbContext, cancellationToken);
            await RunMigrationAsync(usersDbContext, cancellationToken);
            await SeedDataAsync(usersDbContext, cancellationToken);
            logger.LogInformation("Migrated database successfully.");
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
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

    private async Task SeedDataAsync(UsersDbContext usersDbContext, CancellationToken cancellationToken)
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
