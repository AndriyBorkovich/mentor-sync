using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Features.Pipeline;
using MentorSync.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentorSync.Recommendations;

public static class ModuleRegistration
{
    public static void AddRecommendationsModule(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<RecommendationDbContext>(
            connectionName: GeneralConstants.DatabaseName,
            configureSettings: c => c.DisableTracing = true,
            configureDbContextOptions: opt =>
            {
                opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Recommendations));
            });

        builder.Services.AddScoped<IInteractionAggregator, InteractionAggregator>();
        builder.Services.AddScoped<ICollaborativeTrainer, CollaborativeTrainer>();
        builder.Services.AddScoped<IHybridScorer, HybridScorer>();

        // TODO: implement the recommendation pipeline service completely
        //builder.Services.AddHostedService<RecommendationPipelineService>();
    }
}
