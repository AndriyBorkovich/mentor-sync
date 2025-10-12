namespace MentorSync.Notifications.Features.InitiateChat;

/// <summary>
/// Request model to initiate a chat with a recipient
/// </summary>
public sealed class InitiateChatRequest
{
	/// <summary>
	/// Identifier of the recipient to initiate chat with
	/// </summary>
	public int RecipientId { get; set; }
}
