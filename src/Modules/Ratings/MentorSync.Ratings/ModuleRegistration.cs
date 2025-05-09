using MentorSync.Ratings.Data;
using MentorSync.SharedKernel;
using Microsoft.EntityFrameworkCore;
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
    }
}
