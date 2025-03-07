namespace MentorSync.Notifications.Integrations;

public interface IEmailSender
{
    Task SendAsync(string to, string from, string subject, string body);
}
