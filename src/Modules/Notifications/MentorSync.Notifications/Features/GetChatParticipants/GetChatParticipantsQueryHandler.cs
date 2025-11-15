using Ardalis.Result;
using MentorSync.Users.Contracts.Services;

namespace MentorSync.Notifications.Features.GetChatParticipants;

/// <summary>
/// Handler for retrieving chat participants excluding the requesting user
/// </summary>
/// <param name="userService"></param>
public sealed class GetChatParticipantsQueryHandler(IUserService userService)
	: IQueryHandler<GetChatParticipantsQuery, List<GetChatParticipantsResponse>>
{
	/// <inheritdoc />
	public async Task<Result<List<GetChatParticipantsResponse>>> Handle(
		GetChatParticipantsQuery request,
		CancellationToken cancellationToken = default)
	{
		var users = await userService.GetAllUsersExceptAsync(request.UserId);

		var participants = users
			.Select(user => new GetChatParticipantsResponse
			{
				Id = user.Id,
				FullName = user.UserName.Trim(),
				AvatarUrl = user.ProfileImageUrl
			}).ToList();

		return Result.Success(participants);
	}
}
