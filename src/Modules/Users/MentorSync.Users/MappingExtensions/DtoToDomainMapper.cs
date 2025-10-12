using MentorSync.Users.Domain.Mentee;
using MentorSync.Users.Domain.Mentor;
using MentorSync.Users.Features.Mentee.CreateProfile;
using MentorSync.Users.Features.Mentee.EditProfile;
using MentorSync.Users.Features.Mentor.CreateProfile;
using MentorSync.Users.Features.Mentor.EditProfile;

namespace MentorSync.Users.MappingExtensions;

/// <summary>
/// Mapping extensions to convert DTOs to domain models
/// </summary>
public static class DtoToDomainMapper
{
	/// <summary>
	/// Maps a CreateMentorProfileCommand to a MentorProfile domain model
	/// </summary>
	/// <param name="command">The command containing mentor profile data</param>
	/// <returns>A MentorProfile domain model</returns>
	public static MentorProfile ToMentorProfile(this CreateMentorProfileCommand command)
	{
		return new MentorProfile
		{
			Bio = command.Bio,
			Position = command.Position,
			Company = command.Company,
			Industries = command.Industries,
			Skills = command.Skills?.ToList() ?? [],
			ProgrammingLanguages = command.ProgrammingLanguages?.ToList() ?? [],
			ExperienceYears = command.ExperienceYears,
			Availability = command.Availability,
			MentorId = command.MentorId
		};
	}

	/// <summary>
	/// Maps a CreateMenteeProfileCommand to a MenteeProfile domain model
	/// </summary>
	/// <param name="command">The command containing mentee profile data</param>
	/// <returns>A MenteeProfile domain model</returns>
	public static MenteeProfile ToMenteeProfile(this CreateMenteeProfileCommand command)
	{
		return new MenteeProfile
		{
			Bio = command.Bio,
			Position = command.Position,
			Company = command.Company,
			Industries = command.Industries,
			Skills = command.Skills?.ToList() ?? [],
			ProgrammingLanguages = command.ProgrammingLanguages?.ToList() ?? [],
			LearningGoals = command.LearningGoals?.ToList() ?? [],
			MenteeId = command.MenteeId
		};
	}
	/// <summary>
	/// Updates a MentorProfile domain model from an EditMentorProfileCommand
	/// </summary>
	/// <param name="profile">The existing MentorProfile to update</param>
	/// <param name="command">The command containing updated mentor profile data</param>
	/// <returns>Nothing</returns>
	public static void UpdateFrom(this MentorProfile profile, EditMentorProfileCommand command)
	{
		profile.Bio = command.Bio;
		profile.Position = command.Position;
		profile.Company = command.Company;
		profile.Industries = command.Industries;
		profile.Skills = command.Skills?.ToList() ?? [];
		profile.ProgrammingLanguages = command.ProgrammingLanguages?.ToList() ?? [];
		profile.ExperienceYears = command.ExperienceYears;
		profile.Availability = command.Availability;
	}

	/// <summary>
	/// Updates a MenteeProfile domain model from an EditMenteeProfileCommand
	/// </summary>
	/// <param name="profile">The existing MenteeProfile to update</param>
	/// <param name="command">The command containing updated mentee profile data</param>
	/// <returns>Nothing</returns>
	public static void UpdateFrom(this MenteeProfile profile, EditMenteeProfileCommand command)
	{
		profile.Bio = command.Bio;
		profile.Position = command.Position;
		profile.Company = command.Company;
		profile.Industries = command.Industries;
		profile.Skills = command.Skills?.ToList() ?? [];
		profile.ProgrammingLanguages = command.ProgrammingLanguages?.ToList() ?? [];
		profile.LearningGoals = command.LearningGoals?.ToList() ?? [];
	}
}
