using Relaxinema.Core.DTO.GoogleOAUTH;

namespace Relaxinema.Core.ServiceContracts;

public interface IGoogleOAuthService
{
    string GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeChellange);
    Task<TokenResult> ExchangeCodeOnTokenAsync(string code, string codeVerifier, string redirectUrl);
    Task<TokenResult> RefreshTokenAsync(string refreshToken);
}