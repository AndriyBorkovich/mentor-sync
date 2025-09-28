using MentorSync.Notifications.Data;
using MentorSync.Notifications.Features.SendEmail;
using MentorSync.Notifications.Infrastructure.Emails;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentorSync.Notifications;

/// <summary>
/// Registration module for Notifications domain services and dependencies
/// </summary>
public static class ModuleRegistration
{
	/// <summary>
	/// Registers all Notifications module services including SignalR, database, endpoints, and background jobs
	/// </summary>
	/// <param name="builder">The host application builder to configure</param>
	public static void AddNotificationsModule(this IHostApplicationBuilder builder)
	{
		builder.Services.AddSignalR();

		AddDatabase(builder);

		AddEndpoints(builder.Services);

		AddBackgroundJobs(builder);
	}

	/// <summary>
	/// Configures MongoDB database context for the Notifications module
	/// </summary>
	/// <param name="builder">The host application builder</param>
	private static void AddDatabase(IHostApplicationBuilder builder)
	{
		builder.AddMongoDBClient("mongodb");
		builder.Services.Configure<MongoSettings>(
		   builder.Configuration.GetSection(nameof(MongoSettings)));
		builder.Services.AddScoped<NotificationsDbContext>();
	}

	/// <summary>
	/// Registers endpoints and handlers for the Notifications module
	/// </summary>
	/// <param name="services">The service collection to configure</param>
	private static void AddEndpoints(IServiceCollection services)
	{
		services.AddHandlers(typeof(ModuleRegistration).Assembly);
		services.AddEndpoints(typeof(NotificationsDbContext).Assembly);
	}

	/// <summary>
	/// Registers background job services for email processing
	/// </summary>
	/// <param name="builder">The host application builder</param>
	private static void AddBackgroundJobs(IHostApplicationBuilder builder)
	{
		builder.Services.AddSingleton<IEmailSender, AzureEmailSender>();
		builder.Services.AddSingleton<IOutboxProcessor, EmailOutboxProcessor>();
		builder.Services.AddHostedService<EmailSendingJob>();
	}
}
