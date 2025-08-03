
namespace MentorSync.Ratings.Features.MaterialReview.Get;

public sealed class MaterialReviewsResponse
{
	public int ReviewCount { get; set; }
	public double AverageRating { get; set; }
	public IReadOnlyList<MaterialReview> Reviews { get; set; } = [];
}

public sealed record MaterialReview
{
	public int Id { get; set; }
	public int Rating { get; set; }
	public string Comment { get; set; }
	public DateTime CreatedOn { get; set; }
	public string ReviewerName { get; set; }
	public string ReviewerImage { get; set; }
	public bool IsReviewByMentor { get; set; } // Flag to show if review is from mentor or mentee
}
