using FluentValidation;
using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Features.Pipeline;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentorSync.Recommendations;

public static class ModuleRegistration
{
    public static void AddRecommendationsModule(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<RecommendationsDbContext>(
            connectionName: GeneralConstants.DatabaseName,
            configureSettings: c => c.DisableTracing = true,
            configureDbContextOptions: opt =>
            {
                opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Recommendations));
            });

        AddEndpoints(builder.Services);

        builder.Services.AddScoped<IInteractionAggregator, InteractionAggregator>();
        builder.Services.AddScoped<ICollaborativeTrainer, CollaborativeTrainer>();
        builder.Services.AddScoped<IHybridScorer, HybridScorer>();

        builder.Services.AddHostedService<RecommendationPipelineService>();
    }

    private static void AddEndpoints(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ModuleRegistration).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ModuleRegistration).Assembly));
        services.AddEndpoints(typeof(RecommendationsDbContext).Assembly);
    }
}
