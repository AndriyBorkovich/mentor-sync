using MediatR;

namespace MentorSync.Users.Features.GetAllUsers;

public sealed record GetAllUsersQuery(string Role, bool? IsActive) : IRequest<List<UserShortResponse>>;
