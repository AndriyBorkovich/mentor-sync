namespace MentorSync.Notifications.Features.GetAllMessages;

public record GetAllMessagesResponse(
    string Id,
    string To,
    string From,
    string Subject,
    string Body,
    DateTime? DateTimeUtcProcessed);
