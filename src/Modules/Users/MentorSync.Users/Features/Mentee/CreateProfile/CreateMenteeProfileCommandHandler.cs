using Ardalis.Result;
using MediatR;
using MentorSync.Users.Data;
using MentorSync.Users.Features.Common.Responses;
using MentorSync.Users.MappingExtensions;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Features.Mentee.CreateProfile;

public sealed class CreateMenteeProfileCommandHandler(UsersDbContext db)
    : IRequestHandler<CreateMenteeProfileCommand, Result<MenteeProfileResponse>>
{
    public async Task<Result<MenteeProfileResponse>> Handle(CreateMenteeProfileCommand request, CancellationToken cancellationToken)
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
