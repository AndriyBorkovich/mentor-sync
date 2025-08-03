namespace MentorSync.Ratings.Features.MaterialReview.GetForUser;

public sealed record GetUserMaterialReviewQuery(int MaterialId, int UserId)
	: IQuery<UserMaterialReviewResponse>;
