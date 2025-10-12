namespace MentorSync.Notifications.Features.GetChatParticipants;

/// <summary>
/// Response model for chat participants
/// </summary>
public sealed record GetChatParticipantsResponse
{
	/// <summary>
	/// Unique identifier of the participant
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	/// Full name of the participant
	/// </summary>
	public string FullName { get; set; }
	/// <summary>
	/// URL of the participant's avatar image
	/// </summary>
	public string AvatarUrl { get; set; }
}
