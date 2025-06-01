namespace MentorSync.Ratings.Features.MentorReview.Update;

public sealed class UpdateMentorReviewResponse
{
    public int ReviewId { get; set; }
    public int MentorId { get; set; }
    public int MenteeId { get; set; }
    public int Rating { get; set; }
    public string ReviewText { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}
