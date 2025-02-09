using MediatR;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Register;

public sealed class RegisterEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", async (RegisterCommand request, ISender sender) =>
        {
            var result = await sender.Send(request);
            
            return result.DecideWhatToReturn();
        })
        .AllowAnonymous()
        .WithTags("Users")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);;
    }
}