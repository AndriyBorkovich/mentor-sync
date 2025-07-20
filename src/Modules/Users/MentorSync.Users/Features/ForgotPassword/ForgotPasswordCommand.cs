using MentorSync.SharedKernel.Abstractions.Messaging;

namespace MentorSync.Users.Features.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email, string BaseUrl) : ICommand<string>;
