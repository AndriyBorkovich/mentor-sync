using Microsoft.AspNetCore.SignalR;
using MentorSync.Notifications.Data;
using MentorSync.Notifications.Domain;
using MongoDB.Bson;
using System.Security.Claims;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authorization;

namespace MentorSync.Notifications.Infrastructure.Hubs;

[Authorize]
public sealed class NotificationHub(NotificationsDbContext dbContext) : Hub
{
	public async Task SendNotification(string user, string message)
	{
		await Clients.User(user).SendAsync("ReceiveNotification", message, cancellationToken: Context.ConnectionAborted);
	}

	public async Task SendBookingStatusChanged(string userId, string notificationJson)
	{
		await Clients.User(userId).SendAsync("BookingStatusChanged", notificationJson, cancellationToken: Context.ConnectionAborted);
	}

	public async Task SendChatMessage(int recipientId, string content)
	{
		var senderId = GetUserId();

		var chatRoom = await FindOrCreateChatRoom(senderId, recipientId);

		var message = new ChatMessage
		{
			RoomId = chatRoom.Id.ToString(),
			SenderId = senderId,
			Content = content,
			CreatedAt = DateTime.UtcNow,
			IsRead = false,
		};

		await dbContext.ChatMessages.InsertOneAsync(message, cancellationToken: Context.ConnectionAborted);

		var messageDto = new
		{
			id = message.Id.ToString(),
			roomId = message.RoomId,
			senderId = message.SenderId,
			receiverId = recipientId,
			content = message.Content,
			timestamp = message.CreatedAt,
			isRead = message.IsRead,
		};

		await Clients.User(recipientId.ToString()).SendAsync("ReceiveChatMessage", messageDto, cancellationToken: Context.ConnectionAborted);

		await Clients.Caller.SendAsync("MessageSent", messageDto, cancellationToken: Context.ConnectionAborted);
	}

	public async Task MarkMessageAsRead(string messageId)
	{
		var userId = GetUserId();

		var filter = Builders<ChatMessage>.Filter.And(
			Builders<ChatMessage>.Filter.Eq(m => m.Id, ObjectId.Parse(messageId)),
			Builders<ChatMessage>.Filter.Ne(m => m.SenderId, userId) // Only mark messages from others as read
		);

		var update = Builders<ChatMessage>.Update.Set(m => m.IsRead, value: true);
		var result = await dbContext.ChatMessages.UpdateOneAsync(filter, update, cancellationToken: Context.ConnectionAborted);

		if (result.ModifiedCount > 0)
		{
			var message = await dbContext.ChatMessages
				.Find(m => m.Id == ObjectId.Parse(messageId))
				.FirstOrDefaultAsync(cancellationToken: Context.ConnectionAborted);

			if (message != null)
			{
				await Clients.User(message.SenderId.ToString()).SendAsync("MessageRead", messageId, cancellationToken: Context.ConnectionAborted);
			}
		}
	}

	public override async Task OnConnectedAsync()
	{
		var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

		if (!string.IsNullOrEmpty(userId))
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, userId, Context.ConnectionAborted);
		}

		await base.OnConnectedAsync();
	}

	private async Task<ChatRoom> FindOrCreateChatRoom(int user1Id, int user2Id)
	{
		var firstParticipantId = user1Id < user2Id ? user1Id : user2Id;
		var secondParticipantId = user1Id < user2Id ? user2Id : user1Id;

		var filter = Builders<ChatRoom>.Filter.And(
			Builders<ChatRoom>.Filter.Eq(r => r.FirstParticipantId, firstParticipantId),
			Builders<ChatRoom>.Filter.Eq(r => r.SecondParticipantId, secondParticipantId)
		);

		var existingRoom = await dbContext.ChatRooms.Find(filter).FirstOrDefaultAsync(cancellationToken: Context.ConnectionAborted);

		if (existingRoom != null)
		{
			return existingRoom;
		}

		var newRoom = new ChatRoom
		{
			FirstParticipantId = firstParticipantId,
			SecondParticipantId = secondParticipantId,
			CreatedAt = DateTime.UtcNow,
		};

		await dbContext.ChatRooms.InsertOneAsync(newRoom, cancellationToken: Context.ConnectionAborted);
		return newRoom;
	}

	private int GetUserId()
	{
		var userIdClaim = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

		if (Context.User?.Identity?.IsAuthenticated != true)
		{
			throw new HubException("User is not authenticated");
		}

		if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
		{
			throw new HubException("User ID not found or invalid");
		}

		return userId;
	}
}
