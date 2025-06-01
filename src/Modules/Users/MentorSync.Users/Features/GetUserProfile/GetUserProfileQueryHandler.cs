using Ardalis.Result;
using MediatR;
using MentorSync.Users.Data;
using MentorSync.Users.Domain.User;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Features.GetUserProfile;

public sealed class GetUserProfileQueryHandler(
    UserManager<AppUser> userManager,
    UsersDbContext usersDbContext)
    : IRequestHandler<GetUserProfileQuery, Result<UserProfileResponse>>
{
    public async Task<Result<UserProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            return Result.NotFound($"User with ID {request.UserId} not found");
        }

        var role = user.UserRoles?.FirstOrDefault()?.Role?.Name;

        var menteeProfile = await usersDbContext.MenteeProfiles
            .FirstOrDefaultAsync(mp => mp.MenteeId == user.Id, cancellationToken);

        var mentorProfile = await usersDbContext.MentorProfiles
            .FirstOrDefaultAsync(mp => mp.MentorId == user.Id, cancellationToken);

        var response = new UserProfileResponse(
            Id: user.Id,
            UserName: user.UserName,
            Email: user.Email,
            Role: role,
            ProfileImageUrl: user.ProfileImageUrl,
            IsActive: user.IsActive,
            MenteeProfile: menteeProfile != null
                ? new MenteeProfileInfo(
                    menteeProfile.Id,
                    menteeProfile.Bio,
                    menteeProfile.Position,
                    menteeProfile.Company,
                    menteeProfile.Industries.GetCategories(),
                    menteeProfile.Skills,
                    menteeProfile.ProgrammingLanguages,
                    menteeProfile.LearningGoals)
                : null,
            MentorProfile: mentorProfile != null
                ? new MentorProfileInfo(
                    mentorProfile.Id,
                    mentorProfile.Bio,
                    mentorProfile.Position,
                    mentorProfile.Company,
                    mentorProfile.Industries.GetCategories(),
                    mentorProfile.Skills,
                    mentorProfile.ProgrammingLanguages,
                    mentorProfile.ExperienceYears)
                : null
        );

        return Result.Success(response);
    }
}
