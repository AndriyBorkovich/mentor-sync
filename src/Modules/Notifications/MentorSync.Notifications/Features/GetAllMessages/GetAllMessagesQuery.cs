namespace MentorSync.Notifications.Features.GetAllMessages;

/// <summary>
/// Query to retrieve all notification messages
/// </summary>
public sealed record GetAllMessagesQuery : IQuery<List<GetAllMessagesResponse>>;
