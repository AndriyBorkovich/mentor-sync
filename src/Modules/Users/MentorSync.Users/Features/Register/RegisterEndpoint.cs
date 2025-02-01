using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Features.Register;

public sealed class RegisterEndpoint : IEndpoint
{
    public  void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/register", (ILogger<RegisterEndpoint> logger) =>
        {
            logger.LogInformation("Register is called");
        });
    }
}