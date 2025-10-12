using Ardalis.Result;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;
using MentorReviewEntity = MentorSync.Ratings.Domain.MentorReview;

namespace MentorSync.Ratings.Features.MentorReview.Create;

/// <summary>
/// Handler for creating a new mentor review
/// </summary>
/// <param name="dbContext">Database context</param>
public sealed class CreateMentorReviewCommandHandler(
	RatingsDbContext dbContext)
	: ICommandHandler<CreateMentorReviewCommand, CreateMentorReviewResponse>
{
	/// <inheritdoc />
	public async Task<Result<CreateMentorReviewResponse>> Handle(
		CreateMentorReviewCommand command,
		CancellationToken cancellationToken = default)
	{
		// Check if the mentee has already reviewed this mentor
		var existingReview = await dbContext.MentorReviews
			.AsNoTracking()
			.FirstOrDefaultAsync(r => r.MentorId == command.MentorId && r.MenteeId == command.MenteeId,
				cancellationToken);

		if (existingReview != null)
		{
			return Result.Conflict("You have already submitted a review for this mentor");
		}

		var review = new MentorReviewEntity
		{
			MentorId = command.MentorId,
			MenteeId = command.MenteeId,
			Rating = command.Rating,
			ReviewText = command.ReviewText,
			CreatedAt = DateTime.UtcNow,
		};

		dbContext.MentorReviews.Add(review);
		await dbContext.SaveChangesAsync(cancellationToken);

		var response = new CreateMentorReviewResponse
		{
			ReviewId = review.Id,
			MentorId = review.MentorId,
			MenteeId = review.MenteeId,
			Rating = review.Rating,
			ReviewText = review.ReviewText,
			CreatedAt = review.CreatedAt,
		};

		return Result.Success(response);
	}
}
