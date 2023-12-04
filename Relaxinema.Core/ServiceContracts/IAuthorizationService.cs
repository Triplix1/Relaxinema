using Relaxinema.Core.DTO.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Relaxinema.Core.DTO;

namespace Relaxinema.Core.ServiceContracts
{
    public interface IAuthorizationService
    {
        Task<AuthorizationResponse> RegisterUserAsync(RegisterDto registerDto, string[] roles);
        Task<AuthorizationResponse> LoginAsync(LoginDto loginDto);
        Task<AuthorizationResponse> ExternalLogin(ExternalAuthDto externalAuth);
    }
}
