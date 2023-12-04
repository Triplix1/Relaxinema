using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.DTO;
using Relaxinema.Core.DTO.Authorization;
using Relaxinema.WebAPI.Controllers.Base;
using IAuthorizationService = Relaxinema.Core.ServiceContracts.IAuthorizationService;

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
            return Ok(await _authorizationService.RegisterUserAsync(registerDto, new [] {UserRole.User.ToString().ToLower()}));
        }
        
        [HttpPost("register-admin")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<AuthorizationResponse>> RegisterAdmin(RegisterDto registerDto)
        {
            return Ok(await _authorizationService.RegisterUserAsync(registerDto, new [] {UserRole.Admin.ToString().ToLower()}));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthorizationResponse>> Login(LoginDto loginDto)
        {
            return Ok(await _authorizationService.LoginAsync(loginDto));
        }
        
        [HttpPost("external-login")]
        public async Task<IActionResult> ExternalLogin([FromBody]ExternalAuthDto externalAuth)
        {
            return Ok(await _authorizationService.ExternalLogin(externalAuth));
        }
    }
}
