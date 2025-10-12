using System.Globalization;
using System.Security.Claims;
using MentorSync.Notifications.Data;
using MentorSync.Notifications.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MentorSync.Notifications.Infrastructure.Hubs;

/// <summary>
/// SignalR hub for real-time notifications and chat functionality
/// </summary>
/// <param name="dbContext">The notifications database context</param>
[Authorize]
public sealed class NotificationHub(NotificationsDbContext dbContext) : Hub
{
	/// <summary>
	/// Sends a notification to a specific user
	/// </summary>
	/// <param name="user">The target user identifier</param>
	/// <param name="message">The notification message</param>
	/// <returns>A task representing the asynchronous operation</returns>
	public async Task SendNotification(string user, string message)
	{
		await Clients.User(user).SendAsync("ReceiveNotification", message, cancellationToken: Context.ConnectionAborted);
	}

	/// <summary>
	/// Sends a booking status change notification to a specific user
	/// </summary>
	/// <param name="userId">The target user identifier</param>
	/// <param name="notificationJson">The notification data in JSON format</param>
	/// <returns>A task representing the asynchronous operation</returns>
	public async Task SendBookingStatusChanged(string userId, string notificationJson)
	{
		await Clients.User(userId).SendAsync("BookingStatusChanged", notificationJson, cancellationToken: Context.ConnectionAborted);
	}

	/// <summary>
	/// Sends a chat message to a recipient
	/// </summary>
	/// <param name="recipientId">The recipient user identifier</param>
	/// <param name="content">The message content</param>
	/// <returns>A task representing the asynchronous operation</returns>
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

		await Clients.User(recipientId.ToString(CultureInfo.InvariantCulture)).SendAsync("ReceiveChatMessage", messageDto, cancellationToken: Context.ConnectionAborted);

		await Clients.Caller.SendAsync("MessageSent", messageDto, cancellationToken: Context.ConnectionAborted);
	}

	/// <summary>
	/// Marks a specific message as read and notifies the sender
	/// </summary>
	/// <param name="messageId"></param>
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
				await Clients.User(message.SenderId.ToString(CultureInfo.InvariantCulture)).SendAsync("MessageRead", messageId, cancellationToken: Context.ConnectionAborted);
			}
		}
	}

	/// <summary>
	/// Handles a new client connection by adding them to their user-specific group
	/// </summary>
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
