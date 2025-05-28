namespace MentorSync.Ratings.Features.CheckMentorReview;

public sealed class CheckMentorReviewResponse
{
    public bool HasReviewed { get; set; }
    public int? ReviewId { get; set; }
    public int? Rating { get; set; }
    public string ReviewText { get; set; }
}
