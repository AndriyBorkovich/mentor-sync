namespace MentorSync.Users.Features.Common.Responses;

public record AuthResponse(
    string Token,
    string RefreshToken,
    DateTime Expiration);