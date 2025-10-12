namespace MentorSync.Notifications.Features.GetChatParticipants;

/// <summary>
/// Query to get chat participants for a specific user
/// </summary>
/// <param name="UserId">User identifier</param>
public sealed record GetChatParticipantsQuery(int UserId) : IQuery<List<GetChatParticipantsResponse>>;
