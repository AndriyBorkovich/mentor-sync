using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.MaterialReview.GetForUser;

public sealed class GetUserMaterialReviewEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("ratings/materials/{materialId}/user/{userId}/review", async (
            int materialId,
            int userId,
            ISender sender) =>
            {
                var result = await sender.Send(new GetUserMaterialReviewQuery(materialId, userId));

                return result.DecideWhatToReturn();
            })
            .WithTags(TagsConstants.Ratings)
            .WithDescription("Gets a user's review for a specific learning material")
            .Produces<UserMaterialReviewResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
