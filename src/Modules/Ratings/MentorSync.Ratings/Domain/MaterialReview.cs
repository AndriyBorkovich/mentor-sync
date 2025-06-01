namespace MentorSync.Ratings.Domain;

public sealed class MaterialReview : BaseReview
{
    public int MaterialId { get; set; }
    public int ReviewerId { get; set; }
}
