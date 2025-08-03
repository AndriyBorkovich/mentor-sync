using Ardalis.Result;
using MentorSync.Notifications.Data;
using MentorSync.Notifications.Domain;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MentorSync.Notifications.Features.GetChatMessages;

public sealed record GetChatMessagesQuery(string RoomId, int UserId) : IQuery<List<GetChatMessagesResponse>>;

public sealed class GetChatMessagesHandler(NotificationsDbContext dbContext)
	: IQueryHandler<GetChatMessagesQuery, List<GetChatMessagesResponse>>
{
	public async Task<Result<List<GetChatMessagesResponse>>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
	{
		var roomObjectId = ObjectId.Parse(request.RoomId);
		var userId = request.UserId;

		// Verify that the user is a participant in this room
		var roomFilter = Builders<ChatRoom>.Filter.And(
			Builders<ChatRoom>.Filter.Eq(r => r.Id, roomObjectId),
			Builders<ChatRoom>.Filter.Or(
				Builders<ChatRoom>.Filter.Eq(r => r.FirstParticipantId, userId),
				Builders<ChatRoom>.Filter.Eq(r => r.SecondParticipantId, userId)
			)
		);

		var room = await dbContext.ChatRooms.Find(roomFilter).FirstOrDefaultAsync(cancellationToken);
		if (room == null)
		{
			return Result.NotFound("Chat room not found or you don't have access");
		}

		var messagesFilter = Builders<ChatMessage>.Filter.Eq(m => m.RoomId, request.RoomId);
		var messages = await dbContext.ChatMessages
			.Find(messagesFilter)
			.SortBy(m => m.CreatedAt)
			.ToListAsync(cancellationToken);

		// Mark all unread messages from other users as read
		var unreadFilter = Builders<ChatMessage>.Filter.And(
			Builders<ChatMessage>.Filter.Eq(m => m.RoomId, request.RoomId),
			Builders<ChatMessage>.Filter.Ne(m => m.SenderId, userId),
			Builders<ChatMessage>.Filter.Eq(m => m.IsRead, false)
		);

		var update = Builders<ChatMessage>.Update.Set(m => m.IsRead, true);
		await dbContext.ChatMessages.UpdateManyAsync(unreadFilter, update, cancellationToken: cancellationToken);

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
}
