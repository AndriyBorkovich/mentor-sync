using MentorSync.Users.Contracts.Models;
using MentorSync.Users.Contracts.Services;
using MentorSync.Users.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Services;

public sealed class MenteeProfileService(UsersDbContext usersDbContext) : IMenteeProfileService
{
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

	public async Task<IReadOnlyList<MenteePreferences>> GetMenteesPreferences()
	{
		var rnd = new Random();
		return await usersDbContext.MenteeProfiles
			.AsNoTracking()
			.Select(m => new MenteePreferences
			{
				MenteeId = m.MenteeId,
				DesiredProgrammingLanguages = m.ProgrammingLanguages,
				DesiredIndustries = m.Industries,
				MinMentorExperienceYears = rnd.Next(1, 7),
				Position = m.Position,
				DesiredSkills = m.Skills,
			}).ToListAsync();
	}
}
