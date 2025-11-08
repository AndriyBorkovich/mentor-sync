namespace MentorSync.Notifications.Features.GetChatRooms;

/// <summary>
/// Query to retrieve chat rooms for a specific user
/// </summary>
/// <param name="UserId">User identifier</param>
public sealed record GetChatRoomsQuery(int UserId) : IQuery<List<GetChatRoomsResponse>>;
