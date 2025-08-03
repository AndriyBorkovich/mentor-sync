using MentorSync.Ratings.Contracts.Models;

namespace MentorSync.Ratings.Contracts.Services;

public interface IMentorReviewService
{
	double GetAverageRating(int mentorId);
	Task<IReadOnlyList<MentorReviewResult>> GetReviewsByMentorAsync(int mentorId);
	Task<IReadOnlyList<MentorReviewResult>> GetAllReviewsAsync(CancellationToken cancellationToken = default);
}
