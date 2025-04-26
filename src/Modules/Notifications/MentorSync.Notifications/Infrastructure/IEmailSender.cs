namespace MentorSync.Notifications.Infrastructure;

public interface IEmailSender
{
    Task SendAsync(string to, string from, string subject, string body);
}
