using MentorSync.Ratings.Contracts.Models;

namespace MentorSync.Ratings.Contracts;

public interface IMentorReviewService
{
    double GetAverageRating(int mentorId);
    Task<List<MentorReviewResult>> GetReviewsByMentorAsync(int mentorId);
    Task<List<MentorReviewResult>> GetAllReviewsAsync(CancellationToken cancellationToken = default);
}
