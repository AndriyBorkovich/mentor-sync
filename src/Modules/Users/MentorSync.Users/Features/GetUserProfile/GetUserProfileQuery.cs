using Ardalis.Result;
using MediatR;

namespace MentorSync.Users.Features.GetUserProfile;

public sealed record GetUserProfileQuery(int UserId) : IRequest<Result<UserProfileResponse>>;
