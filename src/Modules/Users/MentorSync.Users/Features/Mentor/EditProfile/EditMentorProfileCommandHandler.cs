using Ardalis.Result;
using MentorSync.Users.Data;
using MentorSync.Users.Features.Common.Responses;
using MentorSync.Users.MappingExtensions;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Features.Mentor.EditProfile;

public class EditMentorProfileCommandHandler(UsersDbContext db)
	: ICommandHandler<EditMentorProfileCommand, MentorProfileResponse>
{
	public async Task<Result<MentorProfileResponse>> Handle(EditMentorProfileCommand request, CancellationToken cancellationToken = default)
	{
		var entity = await db.MentorProfiles.FirstOrDefaultAsync(mp => mp.Id == request.Id, cancellationToken: cancellationToken);
		if (entity == null)
			return Result.NotFound($"MentorProfile {request.Id} not found.");

		entity.UpdateFrom(request);
		await db.SaveChangesAsync(cancellationToken);

		var response = entity.ToMentorProfileResponse();

		return Result.Success(response);
	}
}

