
using MentorSync.Users.Contracts.Models;

namespace MentorSync.Users.Contracts.Services;

public interface IUserService
{
    Task<List<UserBasicInfoModel>> GetAllUsersExceptAsync(int userId);
}