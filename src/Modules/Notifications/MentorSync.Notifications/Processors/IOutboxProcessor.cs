namespace MentorSync.Notifications.Processors;

public interface IOutboxProcessor
{
    Task CheckForEmailsToSend();
}