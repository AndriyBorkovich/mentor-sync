namespace MentorSync.Notifications.Features.GetChatParticipants;

public sealed record GetChatParticipantsResponse
{
	public int Id { get; set; }
	public string FullName { get; set; }
	public string AvatarUrl { get; set; }
}
