
using MentorSync.Users.Contracts.Models;

namespace MentorSync.Users.Contracts.Services;

public interface IUserService
{
	Task<IList<UserBasicInfoModel>> GetAllUsersExceptAsync(int userId);
}
