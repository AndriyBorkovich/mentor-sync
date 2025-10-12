using Ardalis.Result;
using MentorSync.Notifications.Data;
using MentorSync.Notifications.Domain;
using MongoDB.Driver;

namespace MentorSync.Notifications.Features.GetChatRooms;

/// <summary>
/// Query to retrieve chat rooms for a specific user
/// </summary>
/// <param name="UserId">User identifier</param>
public sealed record GetChatRoomsQuery(int UserId) : IQuery<List<GetChatRoomsResponse>>;

/// <summary>
/// Handler for retrieving chat rooms for a specific user
/// </summary>
/// <param name="dbContext"></param>
public sealed class GetChatRoomsHandler(NotificationsDbContext dbContext)
	: IQueryHandler<GetChatRoomsQuery, List<GetChatRoomsResponse>>
{
	/// <inheritdoc />
	public async Task<Result<List<GetChatRoomsResponse>>> Handle(
		GetChatRoomsQuery request, CancellationToken cancellationToken = default)
	{
		var userId = request.UserId;
		var filter = Builders<ChatRoom>.Filter.Or(
			Builders<ChatRoom>.Filter.Eq(r => r.FirstParticipantId, userId),
			Builders<ChatRoom>.Filter.Eq(r => r.SecondParticipantId, userId)
		);

		var chatRooms = await dbContext.ChatRooms.Find(filter).ToListAsync(cancellationToken);

		if (chatRooms.Count == 0)
		{
			return Result.Success(new List<GetChatRoomsResponse>());
		}

		var results = new List<GetChatRoomsResponse>();

		foreach (var room in chatRooms)
		{
			var otherParticipantId = room.FirstParticipantId == userId ? room.SecondParticipantId : room.FirstParticipantId;

			var lastMessageFilter = Builders<ChatMessage>.Filter.Eq(m => m.RoomId, room.Id.ToString());
			var lastMessage = await dbContext.ChatMessages
				.Find(lastMessageFilter)
				.SortByDescending(m => m.CreatedAt)
				.FirstOrDefaultAsync(cancellationToken);

			// Count unread messages for the current user
			var unreadFilter = Builders<ChatMessage>.Filter.And(
				Builders<ChatMessage>.Filter.Eq(m => m.RoomId, room.Id.ToString()),
				Builders<ChatMessage>.Filter.Eq(m => m.IsRead, false),
				Builders<ChatMessage>.Filter.Ne(m => m.SenderId, userId) // Messages not sent by the current user
			);
			var unreadCount = await dbContext.ChatMessages.CountDocumentsAsync(unreadFilter, cancellationToken: cancellationToken);

			results.Add(new GetChatRoomsResponse
			{
				Id = room.Id.ToString(),
				ParticipantId = otherParticipantId,
				CreatedAt = room.CreatedAt,
				LastMessage = lastMessage != null
					? new ChatMessageDto
					{
						Id = lastMessage.Id.ToString(),
						SenderId = lastMessage.SenderId,
						Content = lastMessage.Content,
						CreatedAt = lastMessage.CreatedAt,
						IsRead = lastMessage.IsRead
					}
					: null,
				UnreadCount = (int)unreadCount
			});
		}

		return Result.Success(results);
	}
}
