using MentorSync.Ratings.Data;
using MentorSync.Recommendations.Data;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Services;
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
        builder.AddNpgsqlDbContext<UsersDbContext>(
            connectionName: GeneralConstants.DatabaseName,
            configureSettings: c => c.DisableTracing = true,
            configureDbContextOptions: opt =>
            {
                opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Users));
            });

        builder.Services.AddIdentity<AppUser, AppRole>()
            .AddEntityFrameworkStores<UsersDbContext>()
            .AddDefaultTokenProviders();

        builder.AddNpgsqlDbContext<RecommendationDbContext>(
            connectionName: GeneralConstants.DatabaseName,
            configureSettings: c => c.DisableTracing = true,
            configureDbContextOptions: opt =>
            {
                opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Recommendations));
            });

        builder.AddNpgsqlDbContext<RatingsDbContext>(
            connectionName: GeneralConstants.DatabaseName,
            configureSettings: c => c.DisableTracing = true,
            configureDbContextOptions: opt =>
            {
                opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Ratings));
            });
    }

    public static void AddMigrationWorker(this IServiceCollection services)
    {
        services.AddHostedService<Worker>();

        services.AddOpenTelemetry()
            .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));
    }
}
