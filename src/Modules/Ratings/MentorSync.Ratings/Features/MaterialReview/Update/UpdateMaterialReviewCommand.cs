using Ardalis.Result;
using MediatR;

namespace MentorSync.Ratings.Features.MaterialReview.Update;

public sealed record UpdateMaterialReviewCommand(int ReviewId, int ReviewerId, int Rating, string ReviewText)
    : IRequest<Result<Unit>>;
