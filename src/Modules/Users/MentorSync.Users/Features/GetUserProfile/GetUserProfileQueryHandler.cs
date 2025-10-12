using Ardalis.Result;
using MentorSync.Users.Data;
using MentorSync.Users.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Features.GetUserProfile;

/// <summary>
/// Query handler for retrieving a user's profile
/// </summary>
/// <param name="userManager">Identity manager</param>
/// <param name="usersDbContext">Database context</param>
public sealed class GetUserProfileQueryHandler(
	UserManager<AppUser> userManager,
	UsersDbContext usersDbContext)
	: IQueryHandler<GetUserProfileQuery, UserProfileResponse>
{
	/// <inheritdoc />
	public async Task<Result<UserProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken = default)
	{
		var user = await userManager.Users
			.AsNoTracking()
			.Where(u => u.IsActive)
			.Include(u => u.UserRoles)
				.ThenInclude(ur => ur.Role)
			.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

		if (user == null)
		{
			return Result.NotFound($"User with ID {request.UserId} not found");
		}

		var role = user.UserRoles?.FirstOrDefault()?.Role?.Name;

		var menteeProfile = await usersDbContext.MenteeProfiles
			.AsNoTracking()
			.FirstOrDefaultAsync(mp => mp.MenteeId == user.Id, cancellationToken);

		var mentorProfile = await usersDbContext.MentorProfiles
			.AsNoTracking()
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
					menteeProfile.Skills?.ToList() ?? [],
					menteeProfile.ProgrammingLanguages?.ToList() ?? [],
					menteeProfile.LearningGoals?.ToList() ?? [])
				: null,
			MentorProfile: mentorProfile != null
				? new MentorProfileInfo(
					mentorProfile.Id,
					mentorProfile.Bio,
					mentorProfile.Position,
					mentorProfile.Company,
					mentorProfile.Industries.GetCategories(),
					mentorProfile.Skills?.ToList() ?? [],
					mentorProfile.ProgrammingLanguages?.ToList() ?? [],
					mentorProfile.ExperienceYears)
				: null
		);

		return Result.Success(response);
	}
}
