using Ardalis.Result;
using MediatR;
using MentorSync.Users.Data;
using MentorSync.Users.Features.Common.Responses;
using MentorSync.Users.MappingExtensions;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Features.Mentor.EditProfile;

public class EditMentorProfileCommandHandler(UsersDbContext db)
    : IRequestHandler<EditMentorProfileCommand, Result<MentorProfileResponse>>
{
    public async Task<Result<MentorProfileResponse>> Handle(EditMentorProfileCommand request, CancellationToken cancellationToken)
    {
        var entity = await db.MentorProfiles.FirstOrDefaultAsync(mp => mp.Id == request.Id);
        if (entity == null)
            return Result.NotFound($"MentorProfile {request.Id} not found.");

        entity.UpdateFrom(request);
        await db.SaveChangesAsync(cancellationToken);

        var response = entity.ToMentorProfileResponse();

        return Result.Success(response);
    }
}

