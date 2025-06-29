using MentorSync.Users.Contracts.Models;
using MentorSync.Users.Contracts.Services;
using MentorSync.Users.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Services;

public sealed class MenteeProfileService(UsersDbContext usersDbContext) : IMenteeProfileService
{
    public async Task<UserBasicInfoModel> GetMenteeInfo(int menteeId)
    {
        var result = await usersDbContext.Users
        .Select(u => new UserBasicInfoModel
        {
            Id = u.Id,
            UserName = u.UserName,
            ProfileImageUrl = u.ProfileImageUrl,
        })
        .FirstOrDefaultAsync(u => u.Id == menteeId);

        return result;
    }

    public async Task<List<MenteePreferences>> GetMenteesPreferences()
    {
        var rnd = new Random();
        var result = await usersDbContext.MenteeProfiles
            .Select(m => new MenteePreferences
            {
                MenteeId = m.MenteeId,
                DesiredProgrammingLanguages = m.ProgrammingLanguages,
                DesiredIndustries = m.Industries,
                MinMentorExperienceYears = rnd.Next(1, 7),
                Position = m.Position,
                DesiredSkills = m.Skills
            }).ToListAsync();

        return result;
    }
}
