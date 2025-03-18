namespace MentorSync.Notifications.Features.SendEmail;

public interface IOutboxProcessor
{
    Task CheckForEmailsToSend();
}
