using MentorSync.Users.Contracts.Models;
using MentorSync.Users.Contracts.Services;
using MentorSync.Users.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Services;

public sealed class MenteeProfileService(UsersDbContext usersDbContext) : IMenteeProfileService
{
    public async Task<MenteeBasicInfoModel> GetMenteeInfo(int menteeId)
    {
        var result = await usersDbContext.Users
        .Select(u => new MenteeBasicInfoModel
        {
            UserName = u.UserName,
            ProfileImageUrl = u.ProfileImageUrl,
        })
        .FirstOrDefaultAsync(u => u.Id == menteeId);

        return result;
    }
}