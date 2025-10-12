using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MentorSync.Users.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MentorSync.Users.Infrastructure;

/// <inheritdoc />
public sealed class JwtTokenService(
	IOptions<JwtOptions> jwtOptions,
	UserManager<AppUser> userManager)
	: IJwtTokenService
{
	private readonly JwtOptions _jwtOptions = jwtOptions.Value;

	/// <inheritdoc />
	public async Task<TokenResult> GenerateToken(AppUser user)
	{
		var signingCredentials = GetSigningCredentials();
		var claims = await GetClaims(user);
		var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
		var refreshToken = GenerateRefreshToken();
		var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

		return new TokenResult(
			accessToken,
			refreshToken,
			tokenOptions.ValidTo);
	}

	/// <inheritdoc />
	public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
	{
		var tokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = false, // Don't validate lifetime here
			ValidateIssuerSigningKey = true,
			ValidIssuer = _jwtOptions.Issuer,
			ValidAudience = _jwtOptions.Audience,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey))
		};

		try
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token,
				tokenValidationParameters,
				out var securityToken);

			if (securityToken is not JwtSecurityToken jwtSecurityToken ||
				!jwtSecurityToken.Header.Alg.Equals(
					SecurityAlgorithms.HmacSha256,
					StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}

			return principal;
		}
		catch
		{
			return null;
		}
	}

	private static string GenerateRefreshToken()
	{
		var randomNumber = new byte[64];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}

	private SigningCredentials GetSigningCredentials()
	{
		var key = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);
		var secret = new SymmetricSecurityKey(key);
		return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
	}

	private async Task<List<Claim>> GetClaims(AppUser user)
	{
		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, user.Id.ToString(CultureInfo.InvariantCulture)),
			new(ClaimTypes.Email, user.Email!),
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new(ClaimTypes.Name, user.UserName!),
		};

		var roles = await userManager.GetRolesAsync(user);
		claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

		return claims;
	}

	private JwtSecurityToken GenerateTokenOptions(
		SigningCredentials signingCredentials,
		List<Claim> claims)
	{
		var tokenOptions = new JwtSecurityToken(
			issuer: _jwtOptions.Issuer,
			audience: _jwtOptions.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(_jwtOptions.TokenExpirationInMinutes),
			signingCredentials: signingCredentials);

		return tokenOptions;
	}
}
