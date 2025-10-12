using MentorSync.Users.Domain.Mentee;
using MentorSync.Users.Domain.Mentor;
using MentorSync.Users.Domain.User;
using MentorSync.Users.Extensions;
using MentorSync.Users.Features.Common.Responses;
using MentorSync.Users.Features.GetAllUsers;

namespace MentorSync.Users.MappingExtensions;

/// <summary>
/// Extension methods for mapping domain entities to DTOs
/// </summary>
public static class DomainToDtoMapper
{
	/// <summary>
	/// Maps an AppUser entity to a UserShortResponse DTO
	/// </summary>
	/// <param name="user">The AppUser entity to map</param>
	/// <returns>A UserShortResponse DTO</returns>
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

	/// <summary>
	/// Maps a MentorProfile entity to a MentorProfileResponse DTO
	/// </summary>
	/// <param name="profile">Mentor profile entity</param>
	/// <returns>Mentor profile response DTO</returns>
	public static MentorProfileResponse ToMentorProfileResponse(this MentorProfile profile)
	{
		return new MentorProfileResponse(
			profile.Id,
			profile.Industries,
			profile.Skills?.ToList() ?? [],
			profile.ProgrammingLanguages?.ToList() ?? [],
			profile.ExperienceYears,
			profile.Availability.ToReadableString(),
			profile.MentorId);
	}

	/// <summary>
	/// Maps a MenteeProfile entity to a MenteeProfileResponse DTO
	/// </summary>
	/// <param name="profile"></param>
	/// <returns></returns>
	public static MenteeProfileResponse ToMenteeProfileResponse(this MenteeProfile profile)
	{
		return new MenteeProfileResponse(
			profile.Id,
			profile.Bio,
			profile.Position,
			profile.Company,
			profile.Industries,
			profile.Skills?.ToList() ?? [],
			profile.ProgrammingLanguages?.ToList() ?? [],
			profile.LearningGoals?.ToList() ?? [],
			profile.MenteeId);
	}
}
