using FluentValidation;
using MentorSync.Scheduling.Contracts;
using MentorSync.Scheduling.Data;
using MentorSync.Scheduling.Features.Booking.UpdatePending;
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
        AddDatabase(builder);

        AddEndpoints(builder.Services);

        AddExternalServices(builder.Services);

        AddBackgroundJobs(builder.Services);
    }

    private static void AddDatabase(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<SchedulingDbContext>(
            connectionName: GeneralConstants.DatabaseName,
            configureSettings: c => c.DisableTracing = true,
            configureDbContextOptions: opt =>
            {
                opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Scheduling));
            });
    }

    private static void AddEndpoints(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ModuleRegistration).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ModuleRegistration).Assembly));
        services.AddEndpoints(typeof(SchedulingDbContext).Assembly);
    }

    private static void AddExternalServices(this IServiceCollection services)
    {
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<INotificationService, NotificationService>();
    }

    private static void AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddHostedService<UpdatePendingBookingsJob>();
    }
}
