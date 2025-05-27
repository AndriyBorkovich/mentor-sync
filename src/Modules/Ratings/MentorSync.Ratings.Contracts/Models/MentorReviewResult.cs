namespace MentorSync.Ratings.Contracts.Models;

public sealed class MentorReviewResult
{
    public int MentorId { get; set; }
    public int MenteeId { get; set; }
    public int Rating { get; set; }
}
