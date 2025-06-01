namespace MentorSync.Ratings.Features.MentorReview.Create;

public sealed class CreateMentorReviewResponse
{
    public int ReviewId { get; set; }
    public int MentorId { get; set; }
    public int MenteeId { get; set; }
    public int Rating { get; set; }
    public string ReviewText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
