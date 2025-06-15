using Ardalis.Result;
using MediatR;
using MentorSync.Users.Contracts.Services;

namespace MentorSync.Notifications.Features.GetChatParticipants;

public sealed record GetChatParticipantsQuery(int UserId) : IRequest<Result<List<GetChatParticipantsResponse>>>;

public sealed class GetChatParticipantsHandler(IUserService userService) : IRequestHandler<GetChatParticipantsQuery, Result<List<GetChatParticipantsResponse>>>
{
    public async Task<Result<List<GetChatParticipantsResponse>>> Handle(GetChatParticipantsQuery request, CancellationToken cancellationToken)
    {
        var users = await userService.GetAllUsersExceptAsync(request.UserId);

        var participants = users.Select(user => new GetChatParticipantsResponse
        {
            Id = user.Id,
            FullName = user.UserName.Trim(),
            AvatarUrl = user.ProfileImageUrl
        }).ToList();

        return Result.Success(participants);
    }
}
