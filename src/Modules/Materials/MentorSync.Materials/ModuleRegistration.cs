using FluentValidation;
using MentorSync.Materials.Data;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentorSync.Materials;

public static class ModuleRegistration
{
    public static void AddMaterialsModule(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<MaterialsDbContext>(
            connectionName: GeneralConstants.DatabaseName,
            configureSettings: c => c.DisableTracing = true,
            configureDbContextOptions: opt =>
            {
                opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Materials));
            });

        AddEndpoints(builder.Services);
    }

    private static void AddEndpoints(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ModuleRegistration).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ModuleRegistration).Assembly));
        services.AddEndpoints(typeof(MaterialsDbContext).Assembly);
    }
}
