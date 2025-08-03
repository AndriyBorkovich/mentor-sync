using MentorSync.Notifications.Data;
using MentorSync.Notifications.Features.SendEmail;
using MentorSync.Notifications.Infrastructure.Emails;
using MentorSync.SharedKernel.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentorSync.Notifications;

public static class ModuleRegistration
{
	public static void AddNotificationsModule(this IHostApplicationBuilder builder)
	{
		builder.Services.AddSignalR();

		AddDatabase(builder);

		AddEndpoints(builder.Services);

		AddBackgroundJobs(builder);
	}

	private static void AddDatabase(IHostApplicationBuilder builder)
	{
		builder.AddMongoDBClient("mongodb");
		builder.Services.Configure<MongoSettings>(
		   builder.Configuration.GetSection(nameof(MongoSettings)));
		builder.Services.AddScoped<NotificationsDbContext>();
	}

	private static void AddEndpoints(IServiceCollection services)
	{
		services.AddHandlers(typeof(ModuleRegistration).Assembly);
		services.AddEndpoints(typeof(NotificationsDbContext).Assembly);
	}

	private static void AddBackgroundJobs(IHostApplicationBuilder builder)
	{
		builder.Services.AddSingleton<IEmailSender, AzureEmailSender>();
		builder.Services.AddSingleton<IOutboxProcessor, EmailOutboxProcessor>();
		builder.Services.AddHostedService<EmailSendingJob>();
	}
}
