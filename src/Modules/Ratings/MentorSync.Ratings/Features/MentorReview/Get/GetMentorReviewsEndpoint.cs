using MediatR;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.MentorReview.Get;

public sealed class GetMentorReviewsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("ratings/mentors/{id}/reviews", async (
            int id,
            ISender sender) =>
            {
                var result = await sender.Send(new GetMentorReviewsQuery(id));

                return result.DecideWhatToReturn();
            })
            .WithTags("Ratings")
            .WithDescription("Gets reviews for a mentor")
            .Produces<MentorReviewsResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .AllowAnonymous();
    }
}
