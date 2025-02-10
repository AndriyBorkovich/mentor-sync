namespace MentorSync.Users.Infrastructure;

public sealed record TokenResult(
    string AccessToken,
    string RefreshToken,
    DateTime Expiration);