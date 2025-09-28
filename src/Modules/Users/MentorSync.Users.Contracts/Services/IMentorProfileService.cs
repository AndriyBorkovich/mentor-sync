using MentorSync.Users.Contracts.Models;

namespace MentorSync.Users.Contracts.Services;

/// <summary>
/// Service interface for accessing mentor profile information
/// </summary>
public interface IMentorProfileService
{
	/// <summary>
	/// Gets all mentors or specific mentors by their identifiers
	/// </summary>
	/// <param name="mentorIds">Optional array of mentor identifiers to filter by</param>
	/// <returns>A task containing the enumerable collection of mentor profiles</returns>
	Task<IEnumerable<MentorProfileModel>> GetAllMentorsAsync(params int[] mentorIds);
}
