namespace MentorSync.Ratings.Domain;

public sealed class MentorReview : BaseReview
{
	public int MentorId { get; set; }
	public int MenteeId { get; set; }
}
