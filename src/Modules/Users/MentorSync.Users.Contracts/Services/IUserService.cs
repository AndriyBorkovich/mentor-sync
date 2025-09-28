
using MentorSync.Users.Contracts.Models;

namespace MentorSync.Users.Contracts.Services;

/// <summary>
/// Service interface for basic user operations
/// </summary>
public interface IUserService
{
	/// <summary>
	/// Gets all users except the specified user
	/// </summary>
	/// <param name="userId">The user identifier to exclude from results</param>
	/// <returns>A task containing a list of all users except the specified one</returns>
	Task<IList<UserBasicInfoModel>> GetAllUsersExceptAsync(int userId);
}
