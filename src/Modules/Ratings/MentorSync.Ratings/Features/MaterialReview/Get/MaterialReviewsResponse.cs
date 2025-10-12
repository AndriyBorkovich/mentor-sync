namespace MentorSync.Ratings.Features.MaterialReview.Get;

/// <summary>
/// Response model for fetching reviews of a learning material
/// </summary>
public sealed class MaterialReviewsResponse
{
	/// <summary>
	/// The ID of the learning material
	/// </summary>
	public int ReviewCount { get; set; }
	/// <summary>
	/// The average rating of the learning material (1-5 scale)
	/// </summary>
	public double AverageRating { get; set; }
	/// <summary>
	/// List of reviews for the learning material
	/// </summary>
	public IReadOnlyList<MaterialReview> Reviews { get; set; } = [];
}

/// <summary>
/// Represents a review for a learning material
/// </summary>
public sealed record MaterialReview
{
	/// <summary>
	/// Unique identifier for the review
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	/// Rating given in the review (1-5 scale)
	/// </summary>
	public int Rating { get; set; }
	/// <summary>
	/// Textual comment provided in the review
	/// </summary>
	public string Comment { get; set; }
	/// <summary>
	/// Timestamp when the review was created
	/// </summary>
	public DateTime CreatedOn { get; set; }
	/// <summary>
	/// Name of the reviewer (mentee or mentor)
	/// </summary>
	public string ReviewerName { get; set; }
	/// <summary>
	/// URL of the reviewer's profile image
	/// </summary>
	public string ReviewerImage { get; set; }
	/// <summary>
	/// Flag indicating if the review was made by a mentor
	/// </summary>
	public bool IsReviewByMentor { get; set; } // Flag to show if review is from mentor or mentee
}
