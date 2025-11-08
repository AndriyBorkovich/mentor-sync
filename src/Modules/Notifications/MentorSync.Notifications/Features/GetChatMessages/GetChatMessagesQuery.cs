namespace MentorSync.Notifications.Features.GetChatMessages;

/// <summary>
/// Query to get all chat messages in a specific room for a user
/// </summary>
/// <param name="RoomId">Chat room identifier</param>
/// <param name="UserId">User identifier</param>
public sealed record GetChatMessagesQuery(string RoomId, int UserId) : IQuery<List<GetChatMessagesResponse>>;
