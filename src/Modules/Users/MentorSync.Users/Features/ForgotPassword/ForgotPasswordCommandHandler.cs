﻿using Ardalis.Result;
using MediatR;
using MentorSync.Notifications.Contracts;
using MentorSync.SharedKernel;
using MentorSync.Users.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Features.ForgotPassword;

public sealed class ForgotPasswordCommandHandler(
    UserManager<AppUser> userManager,
    IMediator mediator,
    ILogger<ForgotPasswordCommandHandler> logger)
    : IRequestHandler<ForgotPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result.NotFound("User not found");
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var @params = new Dictionary<string, string>
        {
            {"token", token },
            {"email", user.Email }
        };
        var callback = QueryHelpers.AddQueryString($"{request.BaseUrl}/users/reset-password", @params);

        var emailCommand = new SendEmailCommand
        {
            From = GeneralConstants.DefaultEmail,
            To = user.Email,
            Subject = "Reset password",
            Body = $"Hi! Please reset your password by clicking on link below.\n {callback}",
        };

        var result = await mediator.Send(emailCommand, cancellationToken);

        logger.LogInformation("Password reset email was {Result} to user {UserId}", result.IsSuccess ? "sent" : "not sent", user.Id);

        return result.IsSuccess
            ? Result.Success("Password reset email sent")
            : Result.Conflict("Failed to send password reset email");
    }
}
