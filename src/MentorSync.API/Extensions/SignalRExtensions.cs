using MentorSync.Notifications.Infrastructure.Hubs;

namespace MentorSync.API.Extensions;

public static class SignalRExtensions
{
    public static void MapHubs(this WebApplication app)
    {
        app.MapHub<NotificationHub>("/notificationHub");
    }
}
