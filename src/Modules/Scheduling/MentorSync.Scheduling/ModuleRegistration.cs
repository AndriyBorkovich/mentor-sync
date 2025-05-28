using FluentValidation;
using MentorSync.Scheduling.Contracts;
using MentorSync.Scheduling.Data;
using MentorSync.Scheduling.Services;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentorSync.Scheduling;

public static class ModuleRegistration
{
    public static void AddSchedulingModule(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<SchedulingDbContext>(
            connectionName: GeneralConstants.DatabaseName,
            configureSettings: c => c.DisableTracing = true,
            configureDbContextOptions: opt =>
            {
                opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Scheduling));
            });

        AddEndpoints(builder.Services);

        builder.Services.AddScoped<IBookingService, BookingService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
    }

    private static void AddEndpoints(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ModuleRegistration).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ModuleRegistration).Assembly));
        services.AddEndpoints(typeof(SchedulingDbContext).Assembly);
    }
}
