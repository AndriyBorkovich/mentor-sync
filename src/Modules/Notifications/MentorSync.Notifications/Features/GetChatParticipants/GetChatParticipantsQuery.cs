using Ardalis.Result;
using MentorSync.Users.Contracts.Services;

namespace MentorSync.Notifications.Features.GetChatParticipants;

public sealed record GetChatParticipantsQuery(int UserId) : IQuery<List<GetChatParticipantsResponse>>;

public sealed class GetChatParticipantsHandler(IUserService userService)
	: IQueryHandler<GetChatParticipantsQuery, List<GetChatParticipantsResponse>>
{
	public async Task<Result<List<GetChatParticipantsResponse>>> Handle(GetChatParticipantsQuery request, CancellationToken cancellationToken)
	{
		var users = await userService.GetAllUsersExceptAsync(request.UserId);

		var participants = users.ConvertAll(user => new GetChatParticipantsResponse
		{
			Id = user.Id,
			FullName = user.UserName.Trim(),
			AvatarUrl = user.ProfileImageUrl
		});

		return Result.Success(participants);
	}
}
