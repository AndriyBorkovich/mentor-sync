using MentorSync.Ratings.Contracts.Models;

namespace MentorSync.Ratings.Contracts.Services;

/// <summary>
/// Service for managing material reviews
/// </summary>
public interface IMaterialReviewService
{
	/// <summary>
	/// Get mentee material ratings (if any).
	/// </summary>
	Task<IReadOnlyList<MaterialReviewResult>> GetAllReviewsAsync(CancellationToken cancellationToken = default);
}
