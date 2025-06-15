using MentorSync.Users.Contracts.Models;
using MentorSync.Users.Contracts.Services;
using MentorSync.Users.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Services;

public sealed class UserService(UsersDbContext usersDbContext) : IUserService
{
    public async Task<List<UserBasicInfoModel>> GetAllUsersExceptAsync(int userId)
    {
        var result = await usersDbContext.Users
                        .AsNoTracking()
                        .Where(u => u.Id != userId && u.IsActive)
                        .Select(u => new UserBasicInfoModel
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                            ProfileImageUrl = u.ProfileImageUrl,
                        })
                        .ToListAsync();

        return result;
    }
}
