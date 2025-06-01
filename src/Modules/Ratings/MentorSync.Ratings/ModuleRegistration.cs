using FluentValidation;
using MentorSync.Ratings.Contracts.Services;
using MentorSync.Ratings.Data;
using MentorSync.Ratings.Services;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentorSync.Ratings;

public static class ModuleRegistration
{
    public static void AddRatingsModule(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<RatingsDbContext>(
            connectionName: GeneralConstants.DatabaseName,
            configureSettings: c => c.DisableTracing = true,
            configureDbContextOptions: opt =>
            {
                opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Ratings));
            });

        AddEndpoints(builder.Services);

        AddExternalServices(builder.Services);
    }

    private static void AddEndpoints(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ModuleRegistration).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ModuleRegistration).Assembly));
        services.AddEndpoints(typeof(RatingsDbContext).Assembly);
    }

    private static void AddExternalServices(IServiceCollection services)
    {
        services.AddScoped<IMentorReviewService, MentorReviewService>();
        services.AddScoped<IMaterialReviewService, MaterialReviewService>();
    }
}
