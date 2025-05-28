using MentorSync.Users.Contracts.Models;
using MentorSync.Users.Contracts.Services;
using MentorSync.Users.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Services;

public sealed class MentorProfileService(UsersDbContext usersDbContext) : IMentorProfileService
{
    public async Task<IEnumerable<MentorProfileModel>> GetAllMentorsAsync()
    {
        var result = await usersDbContext.MentorProfiles
            .Select(x => new MentorProfileModel
            {
                Id = x.Id,
                ProgrammingLanguages = x.ProgrammingLanguages,
                ExperienceYears = x.ExperienceYears,
                Industry = x.Industries,
            })
            .ToListAsync();

        return result;
    }
}
