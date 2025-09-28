using MentorSync.Users.Contracts.Models;

namespace MentorSync.Users.Contracts.Services;

/// <summary>
/// Service interface for accessing mentee profile information
/// </summary>
public interface IMenteeProfileService
{
	/// <summary>
	/// Gets basic information for a specific mentee
	/// </summary>
	/// <param name="menteeId">The mentee's unique identifier</param>
	/// <returns>A task containing the mentee's basic information</returns>
	Task<UserBasicInfoModel> GetMenteeInfo(int menteeId);

	/// <summary>
	/// Gets preferences for all mentees in the system
	/// </summary>
	/// <returns>A task containing a read-only list of all mentee preferences</returns>
	Task<IReadOnlyList<MenteePreferences>> GetMenteesPreferences();
}
