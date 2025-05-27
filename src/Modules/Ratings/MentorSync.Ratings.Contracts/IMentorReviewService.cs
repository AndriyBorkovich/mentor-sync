namespace MentorSync.Ratings.Contracts
{
    public interface IMentorReviewService
    {
        double GetAverageRating(int mentorId);
    }
}
