using System.ComponentModel.DataAnnotations;
using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Interfaces;
using MentorSync.Users.Domain.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.ResendConfirmation;

public class ResendConfirmationEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/reconfirm", async ([FromQuery, Required] int userId, IMediator mediator) =>
            {
                await mediator.Publish(new UserCreatedEvent(userId));

                return Results.Ok("Confirmation email sent");
            })
            .WithTags(TagsConstants.Users)
            .WithDescription("Resend confirmation email to user")
            .AllowAnonymous()
            .Produces<string>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }
}
