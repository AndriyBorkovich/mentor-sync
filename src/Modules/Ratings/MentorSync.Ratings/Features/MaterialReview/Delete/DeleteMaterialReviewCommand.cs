using Ardalis.Result;
using MediatR;

namespace MentorSync.Ratings.Features.MaterialReview.Delete;

public sealed record DeleteMaterialReviewCommand(int ReviewId, int UserId)
    : IRequest<Result<Unit>>;
