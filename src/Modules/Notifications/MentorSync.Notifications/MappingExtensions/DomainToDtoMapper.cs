using MentorSync.Notifications.Domain;
using MentorSync.Notifications.Features.GetAllMessages;

namespace MentorSync.Notifications.MappingExtensions;

public static class DomainToDtoMapper
{
    public static GetAllMessagesResponse ToResponse(this EmailOutbox emailOutbox)
    {
        return new GetAllMessagesResponse(
            emailOutbox.Id.ToString(),
            emailOutbox.To,
            emailOutbox.From,
            emailOutbox.Subject,
            emailOutbox.DateTimeUtcProcessed);
    }
}
