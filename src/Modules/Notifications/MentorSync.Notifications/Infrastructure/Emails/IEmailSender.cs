namespace MentorSync.Notifications.Infrastructure.Emails;

public interface IEmailSender
{
    Task SendAsync(string to, string from, string subject, string body);
}
