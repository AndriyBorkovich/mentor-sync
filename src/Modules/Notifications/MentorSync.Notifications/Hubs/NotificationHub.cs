using Microsoft.AspNetCore.SignalR;

namespace MentorSync.Notifications.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotification(string user, string message)
    {
        await Clients.User(user).SendAsync("ReceiveNotification", message);
    }

    public async Task SendBookingStatusChanged(string userId, string notificationJson)
    {
        await Clients.User(userId).SendAsync("BookingStatusChanged", notificationJson);
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.Identity?.Name;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }
        await base.OnConnectedAsync();
    }
}
