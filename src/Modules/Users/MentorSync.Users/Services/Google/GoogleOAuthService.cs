using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace MentorSync.Users.Services.Google;

public interface IGoogleOAuthService
{
    string GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeChellange);
    Task<TokenResult> ExchangeCodeOnTokenAsync(string code, string codeVerifier, string redirectUrl);
    Task<TokenResult> RefreshTokenAsync(string refreshToken);
}

public class GoogleOAuthService : IGoogleOAuthService
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _oAuthServerEndpoint;
    private readonly string _tokenServerEndpoint;

    public GoogleOAuthService(IConfiguration configuration)
    {
        _clientId = configuration["GoogleOAuth:ClientId"];
        _clientSecret = configuration["GoogleOAuth:ClientSecret"];
        _oAuthServerEndpoint = configuration["GoogleOAuth:OAuthServerEndpoint"];
        _tokenServerEndpoint = configuration["GoogleOAuth:TokenServerEndpoint"];
    }

    public string GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeChellange)
    {
        var queryParams = new Dictionary<string, string>
            {
                { "client_id", _clientId },
                { "redirect_uri", redirectUrl },
                { "response_type", "code" },
                { "scope", scope },
                { "code_challenge", codeChellange },
                { "code_challenge_method", "S256" },
                { "access_type", "offline" }
            };

        var url = QueryHelpers.AddQueryString(_oAuthServerEndpoint, queryParams);
        return url;
    }

    public async Task<TokenResult> ExchangeCodeOnTokenAsync(string code, string codeVerifier, string redirectUrl)
    {
        var authParams = new Dictionary<string, string>
            {
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "code", code },
                { "code_verifier", codeVerifier },
                { "grant_type", "authorization_code" },
                { "redirect_uri", redirectUrl }
            };

        var tokenResult = await HttpClientHelper.SendPostRequest<TokenResult>(_tokenServerEndpoint, authParams);
        return tokenResult;
    }

    public async Task<TokenResult> RefreshTokenAsync(string refreshToken)
    {
        var refreshParams = new Dictionary<string, string>
            {
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken }
            };

        var tokenResult = await HttpClientHelper.SendPostRequest<TokenResult>(_tokenServerEndpoint, refreshParams);

        return tokenResult;
    }
}
