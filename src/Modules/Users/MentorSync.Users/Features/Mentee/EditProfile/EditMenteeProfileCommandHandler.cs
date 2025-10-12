using Ardalis.Result;
using MentorSync.Users.Data;
using MentorSync.Users.Features.Common.Responses;
using MentorSync.Users.MappingExtensions;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Features.Mentee.EditProfile;

/// <summary>
/// Handler for <see cref="EditMenteeProfileCommand"/>
/// </summary>
/// <param name="db">Database context</param>
public sealed class EditMenteeProfileCommandHandler(UsersDbContext db)
	: ICommandHandler<EditMenteeProfileCommand, MenteeProfileResponse>
{
	/// <inheritdoc />
	public async Task<Result<MenteeProfileResponse>> Handle(EditMenteeProfileCommand request, CancellationToken cancellationToken = default)
	{
		var entity = await db.MenteeProfiles.FirstOrDefaultAsync(mp => mp.Id == request.Id, cancellationToken: cancellationToken);
		if (entity == null)
		{
			return Result.NotFound($"MenteeProfile {request.Id} not found.");
		}

		entity.UpdateFrom(request);
		await db.SaveChangesAsync(cancellationToken);

		var response = entity.ToMenteeProfileResponse();

		return Result.Success(response);
	}
}
