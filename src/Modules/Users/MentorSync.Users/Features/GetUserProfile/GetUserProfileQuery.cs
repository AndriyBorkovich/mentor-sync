namespace MentorSync.Users.Features.GetUserProfile;

public sealed record GetUserProfileQuery(int UserId) : IQuery<UserProfileResponse>;
