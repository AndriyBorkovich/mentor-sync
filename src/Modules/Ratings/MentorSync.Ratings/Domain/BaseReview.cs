using System.ComponentModel.DataAnnotations;

namespace MentorSync.Ratings.Domain;

/// <summary>
/// Base class for reviews, containing common properties
/// </summary>
public class BaseReview
{
	/// <summary>
	/// Unique identifier for the review
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	/// Rating value, constrained between 1 and 5
	/// </summary>
	[Range(1, 5)]
	public int Rating { get; set; }
	/// <summary>
	/// Textual content of the review
	/// </summary>
	public string ReviewText { get; set; }
	/// <summary>
	/// Timestamp of when the review was created
	/// </summary>
	public DateTime CreatedAt { get; set; }
	/// <summary>
	/// Timestamp of when the review was last updated, if applicable
	/// </summary>
	public DateTime? UpdatedAt { get; set; }
}
