namespace MentorSync.Ratings.Domain;

public sealed class ArticleReview : BaseReview
{
    public int ArticleId { get; set; }
    public int ReviewerId { get; set; }
}
