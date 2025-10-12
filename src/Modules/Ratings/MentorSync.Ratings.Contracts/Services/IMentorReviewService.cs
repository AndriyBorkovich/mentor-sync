using MentorSync.Ratings.Contracts.Models;

namespace MentorSync.Ratings.Contracts.Services;

/// <summary>
/// Service for managing mentor reviews
/// </summary>
public interface IMentorReviewService
{
	/// <summary>
	/// Gets the average rating for a mentor
	/// </summary>
	/// <param name="mentorId">Mentor identifier</param>
	/// <returns>Rating value</returns>
	double GetAverageRating(int mentorId);
	/// <summary>
	/// Gets all reviews for a specific mentor
	/// </summary>
	/// <param name="mentorId">Mentor identifier</param>
	/// <returns>List of reviews</returns>
	Task<IReadOnlyList<MentorReviewResult>> GetReviewsByMentorAsync(int mentorId);
	/// <summary>
	/// Gets all mentor reviews
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns>List of reviews</returns>
	Task<IReadOnlyList<MentorReviewResult>> GetAllReviewsAsync(CancellationToken cancellationToken = default);
}
