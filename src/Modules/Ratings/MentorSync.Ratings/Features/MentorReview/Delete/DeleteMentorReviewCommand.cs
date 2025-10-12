namespace MentorSync.Ratings.Features.MentorReview.Delete;

/// <summary>
/// Command to delete a mentor review
/// </summary>
public sealed record DeleteMentorReviewCommand(int ReviewId, int MenteeId): ICommand<string>;
