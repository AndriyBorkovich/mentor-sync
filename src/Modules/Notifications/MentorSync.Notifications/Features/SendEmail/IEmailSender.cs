namespace MentorSync.Notifications.Features.SendEmail;

public interface IEmailSender
{
    Task SendAsync(string to, string from, string subject, string body);
}
