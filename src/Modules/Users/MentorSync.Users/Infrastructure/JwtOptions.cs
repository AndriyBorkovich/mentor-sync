namespace MentorSync.Users.Infrastructure;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";
    
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public int TokenExpirationInMinutes { get; init; } = 60;
    public int RefreshTokenExpirationInDays { get; init; } = 7;
}