using Ardalis.Result;
using MediatR;
using MentorSync.Users.Data;
using MentorSync.Users.Features.Common.Responses;
using MentorSync.Users.MappingExtensions;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Features.Mentor.CreateProfile;

public sealed class CreateMentorProfileCommandHandler(UsersDbContext db)
    : IRequestHandler<CreateMentorProfileCommand, Result<MentorProfileResponse>>
{
    public async Task<Result<MentorProfileResponse>> Handle(CreateMentorProfileCommand request, CancellationToken cancellationToken)
    {
        var profileExists = await db.MentorProfiles
            .AnyAsync(mp => mp.MentorId == request.MentorId, cancellationToken);

        if (profileExists)
        {
            return Result.Conflict("Mentor profile already exists. Edit it instead");
        }

        var entity = request.ToMentorProfile();
        db.MentorProfiles.Add(entity);
        await db.SaveChangesAsync(cancellationToken);

        return Result.Success(entity.ToMentorProfileResponse());
    }
}
