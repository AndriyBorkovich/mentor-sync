using Ardalis.Result;
using MediatR;

namespace MentorSync.Ratings.Features.MaterialReview.GetForUser;

public sealed record GetUserMaterialReviewQuery(int MaterialId, int UserId)
    : IRequest<Result<UserMaterialReviewResponse>>;
