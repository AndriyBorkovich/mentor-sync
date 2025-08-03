using Ardalis.Result;
using MentorSync.Notifications.Data;
using MentorSync.Notifications.Domain;
using MongoDB.Driver;

namespace MentorSync.Notifications.Features.InitiateChat;

public sealed class InitiateChatCommandHandler(NotificationsDbContext dbContext) : ICommandHandler<InitiateChatCommand, InitiateChatResponse>
{
	public async Task<Result<InitiateChatResponse>> Handle(InitiateChatCommand request, CancellationToken cancellationToken = default)
	{
		if (request.SenderId == request.RecipientId)
		{
			return Result.Conflict("Cannot initiate chat with yourself");
		}

		// Ensure consistent ordering of participants
		var firstParticipantId = request.SenderId < request.RecipientId ? request.SenderId : request.RecipientId;
		var secondParticipantId = request.SenderId < request.RecipientId ? request.RecipientId : request.SenderId;

		var filter = Builders<ChatRoom>.Filter.And(
			Builders<ChatRoom>.Filter.Eq(r => r.FirstParticipantId, firstParticipantId),
			Builders<ChatRoom>.Filter.Eq(r => r.SecondParticipantId, secondParticipantId)
		);

		var existingRoom = await dbContext.ChatRooms.Find(filter).FirstOrDefaultAsync(cancellationToken);

		if (existingRoom != null)
		{
			return Result<InitiateChatResponse>.Success(new InitiateChatResponse
			{
				ChatRoomId = existingRoom.Id.ToString()
			});
		}

		var newRoom = new ChatRoom
		{
			FirstParticipantId = firstParticipantId,
			SecondParticipantId = secondParticipantId,
			CreatedAt = DateTime.UtcNow
		};

		await dbContext.ChatRooms.InsertOneAsync(newRoom, null, cancellationToken);

		return Result.Success(new InitiateChatResponse
		{
			ChatRoomId = newRoom.Id.ToString()
		});
	}
}
