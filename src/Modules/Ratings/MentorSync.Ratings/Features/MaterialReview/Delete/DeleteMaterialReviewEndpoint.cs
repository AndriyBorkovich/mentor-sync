using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.MaterialReview.Delete;

public sealed class DeleteMaterialReviewEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("ratings/materials/reviews/{reviewId}", async (
            int reviewId,
            [FromQuery] int userId,
            ISender sender) =>
            {
                var command = new DeleteMaterialReviewCommand(reviewId, userId);

                var result = await sender.Send(command);

                return result.DecideWhatToReturn();
            })
            .WithTags(TagsConstants.Ratings)
            .WithDescription("Deletes a review for a learning material")
            .Produces<Unit>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
