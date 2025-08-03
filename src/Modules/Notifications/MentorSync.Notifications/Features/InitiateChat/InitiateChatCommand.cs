namespace MentorSync.Notifications.Features.InitiateChat;

public sealed record InitiateChatCommand(int SenderId, int RecipientId) : ICommand<InitiateChatResponse>;
