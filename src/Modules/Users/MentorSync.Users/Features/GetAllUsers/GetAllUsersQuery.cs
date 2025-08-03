
namespace MentorSync.Users.Features.GetAllUsers;

public sealed record GetAllUsersQuery(string Role, bool? IsActive) : IQuery<List<UserShortResponse>>;
