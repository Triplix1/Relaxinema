using Relaxinema.Core.DTO.GoogleOAUTH;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.ServiceContracts;

namespace Relaxinema.Core.Services;

public class GoogleOAuthService : IGoogleOAuthService
{
    private readonly string ClientId = "1038141147992-9tbh1kiha40f9cb7s5ve10rttgusfa8k.apps.googleusercontent.com";
    private const string ClientSecret = "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";

        private const string OAuthServerEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        private const string TokenServerEndpoint = "https://oauth2.googleapis.com/token";

        public string GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeChellange)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "client_id", ClientId },
                { "redirect_uri", redirectUrl },
                { "response_type", "code" },
                { "scope", scope },
                { "code_challenge", codeChellange },
                { "code_challenge_method", "S256" },
                { "access_type", "offline" }
            };

            var url = QueryHelper.BuildUrlWithQueryStringUsingStringConcat(OAuthServerEndpoint, queryParams);
            return url;
        }

        public async Task<TokenResult> ExchangeCodeOnTokenAsync(string code, string codeVerifier, string redirectUrl)
        {
            var authParams = new Dictionary<string, string>
            {
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "code", code },
                { "code_verifier", codeVerifier },
                { "grant_type", "authorization_code" },
                { "redirect_uri", redirectUrl }
            };

            var tokenResult = await HttpClientHelper.SendPostRequest<TokenResult>(TokenServerEndpoint, authParams);
            return tokenResult;
        }

        public async Task<TokenResult> RefreshTokenAsync(string refreshToken)
        {
            var refreshParams = new Dictionary<string, string>
            {
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken }
            };

            var tokenResult = await HttpClientHelper.SendPostRequest<TokenResult>(TokenServerEndpoint, refreshParams);

            return tokenResult;
        }
}