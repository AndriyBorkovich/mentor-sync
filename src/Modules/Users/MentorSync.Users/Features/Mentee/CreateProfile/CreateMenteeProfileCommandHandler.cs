using Ardalis.Result;
using MentorSync.Users.Data;
using MentorSync.Users.Features.Common.Responses;
using MentorSync.Users.MappingExtensions;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Features.Mentee.CreateProfile;

/// <summary>
/// Command handler for creating a new mentee profile.
/// </summary>
/// <param name="db">Database context</param>
public sealed class CreateMenteeProfileCommandHandler(UsersDbContext db)
	: ICommandHandler<CreateMenteeProfileCommand, MenteeProfileResponse>
{
	/// <inheritdoc />
	public async Task<Result<MenteeProfileResponse>> Handle(CreateMenteeProfileCommand request, CancellationToken cancellationToken = default)
	{
		var profileExists = await db.MenteeProfiles
			.AnyAsync(mp => mp.MenteeId == request.MenteeId, cancellationToken);

		if (profileExists)
		{
			return Result.Conflict("Mentee profile already exists. Edit it instead");
		}

		var entity = request.ToMenteeProfile();
		db.MenteeProfiles.Add(entity);
		await db.SaveChangesAsync(cancellationToken);

		var response = entity.ToMenteeProfileResponse();

		return Result.Success(response);
	}
}
