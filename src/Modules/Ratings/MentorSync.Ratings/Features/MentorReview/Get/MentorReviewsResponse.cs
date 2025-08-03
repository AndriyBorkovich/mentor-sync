namespace MentorSync.Ratings.Features.MentorReview.Get;

public sealed record MentorReviewsResponse
{
	public int ReviewCount { get; init; }
	public IReadOnlyList<MentorReview> Reviews { get; init; } = [];
}

public sealed record MentorReview
{
	public int Id { get; init; }
	public string ReviewerName { get; init; }
	public string ReviewerImage { get; init; }
	public int Rating { get; init; }
	public string Comment { get; init; }
	public DateTime CreatedOn { get; init; }
}
