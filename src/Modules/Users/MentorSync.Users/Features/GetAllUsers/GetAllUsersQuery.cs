
namespace MentorSync.Users.Features.GetAllUsers;

/// <summary>
/// Query to get all users with optional role and active status filtering
/// </summary>
public sealed record GetAllUsersQuery(string Role, bool? IsActive) : IQuery<List<UserShortResponse>>;
