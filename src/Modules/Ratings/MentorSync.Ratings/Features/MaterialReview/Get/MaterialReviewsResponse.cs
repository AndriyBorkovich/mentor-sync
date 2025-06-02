
namespace MentorSync.Ratings.Features.MaterialReview.Get;

public sealed class MaterialReviewsResponse
{
    public int ReviewCount { get; set; }
    public double AverageRating { get; set; }
    public List<MaterialReview> Reviews { get; set; } = new();
}
