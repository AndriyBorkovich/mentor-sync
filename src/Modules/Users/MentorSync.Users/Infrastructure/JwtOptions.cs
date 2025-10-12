namespace MentorSync.Users.Infrastructure;

/// <summary>
/// Options for JWT (JSON Web Token) configuration
/// </summary>
public sealed class JwtOptions
{
	/// <summary>
	/// The section name in the configuration file
	/// </summary>
	public const string SectionName = "Jwt";

	/// <summary>
	/// The issuer of the JWT
	/// </summary>
	public string Issuer { get; init; } = string.Empty;
	/// <summary>
	/// The audience for the JWT
	/// </summary>
	public string Audience { get; init; } = string.Empty;
	/// <summary>
	/// The secret key used for signing the JWT
	/// </summary>
	public string SecretKey { get; init; } = string.Empty;
	/// <summary>
	/// The expiration time for the JWT in minutes
	/// </summary>
	public int TokenExpirationInMinutes { get; init; }
	/// <summary>
	/// The expiration time for the refresh token in days
	/// </summary>
	public int RefreshTokenExpirationInDays { get; init; }
}
