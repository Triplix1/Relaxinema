using Relaxinema.Core.DTO.Authorization;

namespace Relaxinema.Core.ServiceContracts
{
    public interface IAuthorizationService
    {
        Task<AuthorizationResponse> RegisterUserAsync(RegisterDto registerDto, string[] roles);
        Task<AuthorizationResponse> LoginAsync(LoginDto loginDto);
        Task<AuthorizationResponse> ExternalLogin(ExternalAuthDto externalAuth);
    }
}
