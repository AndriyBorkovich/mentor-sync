namespace MentorSync.Users.Features.GetUserProfile;

/// <summary>
/// Query to get a user's profile by their user ID
/// </summary>
public sealed record GetUserProfileQuery(int UserId) : IQuery<UserProfileResponse>;
