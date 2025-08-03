using MentorSync.Users.Domain.Mentee;
using MentorSync.Users.Domain.Mentor;
using MentorSync.Users.Domain.User;
using MentorSync.Users.Extensions;
using MentorSync.Users.Features.Common.Responses;
using MentorSync.Users.Features.GetAllUsers;

namespace MentorSync.Users.MappingExtensions;

public static class DomainToDtoMapper
{
	public static UserShortResponse ToUserShortResponse(this AppUser user)
	{
		return new UserShortResponse(
			user.Id,
			user.UserName,
			user.Email,
			user.UserRoles?.FirstOrDefault()?.Role?.Name,
			user.ProfileImageUrl,
			user.IsActive,
			user.EmailConfirmed);
	}
	public static MentorProfileResponse ToMentorProfileResponse(this MentorProfile profile)
	{
		return new MentorProfileResponse(
			profile.Id,
			profile.Industries,
			profile.Skills,
			profile.ProgrammingLanguages,
			profile.ExperienceYears,
			profile.Availability.ToReadableString(),
			profile.MentorId);
	}

	public static MenteeProfileResponse ToMenteeProfileResponse(this MenteeProfile profile)
	{
		return new MenteeProfileResponse(
			profile.Id,
			profile.Bio,
			profile.Position,
			profile.Company,
			profile.Industries,
			profile.Skills,
			profile.ProgrammingLanguages,
			profile.LearningGoals,
			profile.MenteeId);
	}
}
