using MentorSync.Notifications.Hubs;

namespace MentorSync.API.Extensions;

public static class SignalRExtensions
{
    public static void MapSignalRHubs(this WebApplication app)
    {
        app.MapHub<NotificationHub>("/notificationHub");
    }
}
