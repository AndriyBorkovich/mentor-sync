using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.MaterialReview.Get;

public sealed class GetMaterialReviewsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("ratings/materials/{id}/reviews", async (
            int id,
            ISender sender) =>
            {
                var result = await sender.Send(new GetMaterialReviewsQuery(id));

                return result.DecideWhatToReturn();
            })
            .WithTags(TagsConstants.Ratings)
            .WithDescription("Gets reviews for a learning material")
            .Produces<MaterialReviewsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .AllowAnonymous();
    }
}
