using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Relaxinema.Core.DTO.Authorization;
using Relaxinema.Core.ServiceContracts;
using Relaxinema.WebAPI.Controllers.Base;

namespace Relaxinema.WebAPI.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAuthorizationService _authorizationService;
        public AccountController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthorizationResponse>> Register(RegisterDto registerDto)
        {
            return Ok(await _authorizationService.RegisterUserAsync(registerDto));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthorizationResponse>> Login(LoginDto loginDto)
        {
            return Ok(await _authorizationService.LoginAsync(loginDto));
        }
    }
}
