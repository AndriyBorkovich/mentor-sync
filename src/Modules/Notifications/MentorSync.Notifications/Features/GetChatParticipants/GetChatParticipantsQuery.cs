namespace MentorSync.Notifications.Features.GetChatParticipants;

public sealed record GetChatParticipantsQuery(int UserId) : IQuery<List<GetChatParticipantsResponse>>;
