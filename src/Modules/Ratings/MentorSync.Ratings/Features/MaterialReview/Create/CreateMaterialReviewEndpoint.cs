using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.MaterialReview.Create;

public sealed class CreateMaterialReviewEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("ratings/materials/{materialId}/reviews", async (
            int materialId,
            [FromBody] CreateMaterialReviewRequest request,
            ISender sender) =>
            {
                var command = new CreateMaterialReviewCommand(
                    materialId,
                    request.ReviewerId,
                    request.Rating,
                    request.ReviewText);

                var result = await sender.Send(command);

                return result.DecideWhatToReturn();
            })
            .WithTags(TagsConstants.Ratings)
            .WithDescription("Creates a new review for a learning material")
            .Produces<CreateMaterialReviewResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status403Forbidden);
    }
}

public sealed record CreateMaterialReviewRequest(int ReviewerId, int Rating, string ReviewText);
