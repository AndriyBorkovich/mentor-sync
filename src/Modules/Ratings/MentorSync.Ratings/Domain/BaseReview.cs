using System.ComponentModel.DataAnnotations;

namespace MentorSync.Ratings.Domain;

public class BaseReview
{
	public int Id { get; set; }

	[Range(1, 5)]
	public int Rating { get; set; }

	public string ReviewText { get; set; }

	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
}
