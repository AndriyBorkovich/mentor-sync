using Ardalis.Result;
using MediatR;

namespace MentorSync.Ratings.Features.MaterialReview.Get;

public sealed record GetMaterialReviewsQuery(int MaterialId)
    : IRequest<Result<MaterialReviewsResponse>>;
