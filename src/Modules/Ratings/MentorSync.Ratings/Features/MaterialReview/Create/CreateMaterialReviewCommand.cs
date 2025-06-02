using Ardalis.Result;
using MediatR;

namespace MentorSync.Ratings.Features.MaterialReview.Create;

public sealed record CreateMaterialReviewCommand(int MaterialId, int ReviewerId, int Rating, string ReviewText)
    : IRequest<Result<CreateMaterialReviewResponse>>;
