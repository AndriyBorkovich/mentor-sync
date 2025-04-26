using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.GetAllUsers;

public sealed class GetAllUsersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users", async ([FromQuery] string role, [FromQuery] bool? isActive, ISender sender)
                        => await sender.Send(new GetAllUsersQuery(role, isActive)))
            .WithTags(TagsConstants.Users)
            .WithDescription("Get all users")
            .Produces<List<UserShortResponse>>();
    }
}
