namespace MentorSync.Notifications.Features.InitiateChat;

/// <summary>
/// Command to initiate a chat between a sender and a recipient
/// </summary>
/// <param name="SenderId">Sender Id</param>
/// <param name="RecipientId">Recipient Id</param>
public sealed record InitiateChatCommand(int SenderId, int RecipientId) : ICommand<InitiateChatResponse>;
