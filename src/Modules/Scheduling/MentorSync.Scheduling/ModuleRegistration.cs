using FluentValidation;
using MentorSync.Scheduling.Contracts.Services;
using MentorSync.Scheduling.Data;
using MentorSync.Scheduling.Features.Booking.UpdatePending;
using MentorSync.Scheduling.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentorSync.Scheduling;

/// <summary>
/// Registration module for Scheduling domain services and dependencies
/// </summary>
public static class ModuleRegistration
{
	/// <summary>
	/// Registers all Scheduling module services including database context, endpoints, external services, and background jobs
	/// </summary>
	/// <param name="builder">The host application builder to configure</param>
	public static void AddSchedulingModule(this IHostApplicationBuilder builder)
	{
		AddDatabase(builder);

		AddEndpoints(builder.Services);

		AddExternalServices(builder.Services);

		AddBackgroundJobs(builder.Services);
	}

	/// <summary>
	/// Configures the PostgreSQL database context for the Scheduling module
	/// </summary>
	/// <param name="builder">The host application builder</param>
	private static void AddDatabase(this IHostApplicationBuilder builder)
	{
		builder.AddNpgsqlDbContext<SchedulingDbContext>(
			connectionName: GeneralConstants.DatabaseName,
			configureSettings: c => c.DisableTracing = true,
			configureDbContextOptions: opt =>
				opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Scheduling)));
	}

	/// <summary>
	/// Registers endpoints, validators, and handlers for the Scheduling module
	/// </summary>
	/// <param name="services">The service collection to configure</param>
	private static void AddEndpoints(IServiceCollection services)
	{
		var assembly = typeof(ModuleRegistration).Assembly;
		services.AddValidatorsFromAssembly(assembly);
		services.AddHandlers(assembly);
		services.AddEndpoints(assembly);
	}

	/// <summary>
	/// Registers external services exposed by the Scheduling module
	/// </summary>
	/// <param name="services">The service collection to configure</param>
	private static void AddExternalServices(IServiceCollection services)
	{
		services.AddScoped<IBookingService, BookingService>();
		services.AddScoped<INotificationService, NotificationService>();
	}

	/// <summary>
	/// Registers background job services for booking management
	/// </summary>
	/// <param name="services">The service collection to configure</param>
	private static void AddBackgroundJobs(IServiceCollection services)
	{
		services.AddHostedService<UpdatePendingBookingsJob>();
	}
}
