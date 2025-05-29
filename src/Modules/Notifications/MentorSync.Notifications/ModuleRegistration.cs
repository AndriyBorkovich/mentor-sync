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
        builder.AddMongoDBClient("mongodb");
        builder.Services.Configure<MongoSettings>(
           builder.Configuration.GetSection(nameof(MongoSettings)));
        builder.Services.AddSingleton<NotificationsDbContext>();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ModuleRegistration).Assembly));
        builder.Services.AddEndpoints(typeof(NotificationsDbContext).Assembly);

        builder.Services.AddSingleton<IEmailSender, AzureEmailSender>();
        builder.Services.AddSingleton<IOutboxProcessor, EmailOutboxProcessor>();
        builder.Services.AddHostedService<EmailSendingJob>();

        builder.Services.AddSignalR();
    }
}
