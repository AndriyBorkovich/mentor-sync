using Ardalis.Result;
using MentorSync.Notifications.Data;
using MentorSync.Notifications.Domain;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MentorSync.Notifications.Features.GetChatMessages;

/// <summary>
/// Query to get all chat messages in a specific room for a user
/// </summary>
/// <param name="RoomId">Chat room identifier</param>
/// <param name="UserId">User identifier</param>
public sealed record GetChatMessagesQuery(string RoomId, int UserId) : IQuery<List<GetChatMessagesResponse>>;

/// <summary>
/// Handler for processing GetChatMessagesQuery
/// </summary>
/// <param name="dbContext">Database context</param>
public sealed class GetChatMessagesHandler(NotificationsDbContext dbContext)
	: IQueryHandler<GetChatMessagesQuery, List<GetChatMessagesResponse>>
{
	/// <inheritdoc />
	public async Task<Result<List<GetChatMessagesResponse>>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken = default)
	{
		var userId = request.UserId;
		var roomId = request.RoomId;
		var (room, validationResult) = await TryGetRoomAsync(userId, roomId, cancellationToken);
		if (!validationResult.IsSuccess)
		{
			return validationResult;
		}

		var messages = await UpdateUnreadMessagesAsync(userId, roomId, cancellationToken);
		var otherParticipantId = room.FirstParticipantId == userId ? room.SecondParticipantId : room.FirstParticipantId;
		var result = messages.ConvertAll(m => new GetChatMessagesResponse
		{
			Id = m.Id.ToString(),
			SenderId = m.SenderId,
			ReceiverId = m.SenderId == userId ? otherParticipantId : userId,
			Content = m.Content,
			Timestamp = m.CreatedAt,
			IsRead = m.IsRead
		});

		return Result<List<GetChatMessagesResponse>>.Success(result);
	}

	private async Task<List<ChatMessage>> UpdateUnreadMessagesAsync(int userId, string roomId, CancellationToken cancellationToken)
	{
		var messagesFilter = Builders<ChatMessage>.Filter.Eq(m => m.RoomId, roomId);
		var messages = await dbContext.ChatMessages
			.Find(messagesFilter)
			.SortBy(m => m.CreatedAt)
			.ToListAsync(cancellationToken);

		// Mark all unread messages from other users as read
		var unreadFilter = Builders<ChatMessage>.Filter.And(
			Builders<ChatMessage>.Filter.Eq(m => m.RoomId, roomId),
			Builders<ChatMessage>.Filter.Ne(m => m.SenderId, userId),
			Builders<ChatMessage>.Filter.Eq(m => m.IsRead, value: false)
		);

		var update = Builders<ChatMessage>.Update.Set(m => m.IsRead, value: true);
		await dbContext.ChatMessages.UpdateManyAsync(unreadFilter, update, cancellationToken: cancellationToken);

		return messages;
	}

	private async Task<(ChatRoom room, Result<List<GetChatMessagesResponse>> result)> TryGetRoomAsync(
		int userId,
		string roomId,
		CancellationToken cancellationToken)
	{
		var roomObjectId = ObjectId.Parse(roomId);
		var roomFilter = Builders<ChatRoom>.Filter.And(
			Builders<ChatRoom>.Filter.Eq(r => r.Id, roomObjectId),
			Builders<ChatRoom>.Filter.Or(
				Builders<ChatRoom>.Filter.Eq(r => r.FirstParticipantId, userId),
				Builders<ChatRoom>.Filter.Eq(r => r.SecondParticipantId, userId)));

		var room = await dbContext.ChatRooms.Find(roomFilter).FirstOrDefaultAsync(cancellationToken);
		return room is null ? (null, Result.NotFound("Chat room not found or you don't have access")) : (room, Result.Success());
	}
}
