using MentorSync.Ratings.Contracts.Models;
using MentorSync.Ratings.Contracts.Services;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Ratings.Services;

internal sealed class MentorReviewService(RatingsDbContext ratingsDbContext) : IMentorReviewService
{
    public double GetAverageRating(int mentorId)
    {
        return ratingsDbContext.MentorReviews
            .Where(review => review.MentorId == mentorId)
            .Average(review => review.Rating);
    }

    public Task<List<MentorReviewResult>> GetReviewsByMentorAsync(int mentorId)
    {
        var result = ratingsDbContext.MentorReviews
            .Where(review => review.MentorId == mentorId)
            .Select(review => new MentorReviewResult
            {
                MentorId = review.MentorId,
                MenteeId = review.MenteeId,
                Rating = review.Rating
            })
            .ToListAsync();

        return result;
    }

    public Task<List<MentorReviewResult>> GetAllReviewsAsync(CancellationToken cancellationToken = default)
    {
        var result = ratingsDbContext.MentorReviews
            .Select(review => new MentorReviewResult
            {
                MentorId = review.MentorId,
                MenteeId = review.MenteeId,
                Rating = review.Rating
            })
            .ToListAsync(cancellationToken);

        return result;
    }
}
