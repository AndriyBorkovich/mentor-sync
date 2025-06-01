namespace MentorSync.Ratings.Contracts.Models;

/// <summary>
/// Model representing the result of a mentor review from a mentee
/// </summary>
public sealed class MentorReviewResult
{
    public int MentorId { get; set; }
    public int MenteeId { get; set; }
    public int Rating { get; set; }
}
