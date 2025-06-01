using System.ComponentModel.DataAnnotations;

namespace MentorSync.Ratings.Features.MentorReview.Create;

public sealed class CreateMentorReviewRequest
{
    [Required]
    public int MentorId { get; set; }

    [Range(1, 5)]
    [Required]
    public int Rating { get; set; }

    [Required]
    [StringLength(1000, MinimumLength = 10)]
    public string ReviewText { get; set; } = string.Empty;
}
