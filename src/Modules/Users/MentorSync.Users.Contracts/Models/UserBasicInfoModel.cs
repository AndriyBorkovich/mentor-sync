namespace MentorSync.Users.Contracts.Models;

/// <summary>
/// Basic user information model for cross-module access
/// </summary>
public sealed class UserBasicInfoModel
{
	/// <summary>
	/// Gets or sets the user's unique identifier
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the user's username
	/// </summary>
	public string UserName { get; set; }

	/// <summary>
	/// Gets or sets the user's profile image URL
	/// </summary>
	public string ProfileImageUrl { get; set; }
}