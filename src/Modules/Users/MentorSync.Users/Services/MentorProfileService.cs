using MentorSync.Users.Contracts.Models;
using MentorSync.Users.Contracts.Services;
using MentorSync.Users.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Services;

/// <inheritdoc />
public sealed class MentorProfileService(UsersDbContext usersDbContext) : IMentorProfileService
{
	/// <inheritdoc />
	public async Task<IEnumerable<MentorProfileModel>> GetAllMentorsAsync(params int[] mentorIds)
	{
		var query = usersDbContext.MentorProfiles.AsNoTracking();
		if (mentorIds.Length > 0)
		{
			query = query.Where(x => mentorIds.Contains(x.Id));
		}

		var result = await query
			.Select(x => new MentorProfileModel
			{
				Id = x.Id,
				UserName = x.User.UserName,
				ProgrammingLanguages = x.ProgrammingLanguages,
				ExperienceYears = x.ExperienceYears,
				Industry = x.Industries,
				Position = x.Position,
				Skills = x.Skills.ToList()
			})
			.ToListAsync();

		return result;
	}
}
