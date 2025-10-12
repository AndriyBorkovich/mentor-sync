using MentorSync.Users.Contracts.Models;
using MentorSync.Users.Contracts.Services;
using MentorSync.Users.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Services;

/// <summary>
/// Service for retrieving mentee profile information
/// </summary>
/// <param name="usersDbContext">Database context</param>
public sealed class MenteeProfileService(UsersDbContext usersDbContext) : IMenteeProfileService
{
	/// <inheritdoc />
	public async Task<UserBasicInfoModel> GetMenteeInfo(int menteeId)
	{
		return await usersDbContext.Users
						.AsNoTracking()
						.Select(u => new UserBasicInfoModel
						{
							Id = u.Id,
							UserName = u.UserName,
							ProfileImageUrl = u.ProfileImageUrl,
						})
						.FirstOrDefaultAsync(u => u.Id == menteeId);
	}

	/// <inheritdoc />
	public async Task<IReadOnlyList<MenteePreferences>> GetMenteesPreferences()
	{
		var rnd = new Random();
		return await usersDbContext.MenteeProfiles
			.AsNoTracking()
			.Select(m => new MenteePreferences
			{
				MenteeId = m.MenteeId,
				DesiredProgrammingLanguages = m.ProgrammingLanguages.ToList(),
				DesiredIndustries = m.Industries,
				MinMentorExperienceYears = rnd.Next(1, 7),
				Position = m.Position,
				DesiredSkills = m.Skills.ToList(),
			}).ToListAsync();
	}
}
