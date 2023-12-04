using AutoMapper;
using Relaxinema.Core.Exceptions;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Core.ServiceContracts;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Relaxinema.Core.DTO;
using Relaxinema.Core.DTO.User;
using Relaxinema.Core.DTO.Authorization;

namespace Relaxinema.Core.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly JwtHelper _jwtHelper;

        public AuthorizationService(UserManager<User> userManager, ITokenService tokenService, IMapper mapper, RoleManager<IdentityRole<Guid>> roleManager, JwtHelper jwtHelper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _roleManager = roleManager;
            _jwtHelper = jwtHelper;
        }
        
        public async Task<AuthorizationResponse> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) throw new AuthorizationException("Invalid Username");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
            {
                throw new AuthorizationException("Invalid Password");
            }

            return new AuthorizationResponse
            {
                Nickname = user.Nickname,
                Token = await _tokenService.CreateTokenAsync(user)
            };
        }

        public async Task<AuthorizationResponse> RegisterUserAsync(RegisterDto registerDto, string[] roles)
        {
            var userWithSameNickname = await _userManager.Users.FirstOrDefaultAsync(u => u.Nickname == registerDto.Nickname);

            if (userWithSameNickname != null)
                throw new AuthorizationException("Here is already user with the same nickname");
            

            var userWithSameEmail = await _userManager.FindByEmailAsync(registerDto.Email);

            if (userWithSameEmail != null)
                throw new AuthorizationException("Here is already user with the same email");

            var user = _mapper.Map<User>(registerDto);
            
            user.Nickname = registerDto.Nickname.ToLower();
            user.UserName = user.Nickname;
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                throw new AuthorizationException(string.Join(",", result.Errors.SelectMany(e => e.Description)));

            foreach (var role in roles)
            {
                var containsRole = await _roleManager.FindByNameAsync(role) != null;
                
                if (!containsRole)
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(){Name = role, NormalizedName = role.ToLower()});
                
                var roleResult = await _userManager.AddToRoleAsync(user, role);
                
                if(!roleResult.Succeeded)
                    throw new AuthorizationException(string.Join(",", roleResult.Errors.SelectMany(e => e.Description)));

            }

            return new AuthorizationResponse
            {
                Nickname = user.Nickname,
                Token = await _tokenService.CreateTokenAsync(user)
            };
        }
        
        public async Task<AuthorizationResponse> ExternalLogin(ExternalAuthDto externalAuth)
        {
            var payload =  await _jwtHelper.VerifyGoogleToken(externalAuth);
        
            if(payload == null)
                throw new ArgumentException("Invalid External Authentication.");
            
            var info = new UserLoginInfo(externalAuth.Provider, payload.Subject, externalAuth.Provider);
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user is null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                
                if (user is null)
                {
                    user = new User { Email = payload.Email, Nickname = payload.Email, UserName = payload.Email, PhotoUrl = payload.Picture};
                    var creationResult = await _userManager.CreateAsync(user);
                    if (!creationResult.Succeeded)
                    {
                        throw new ArgumentException("Invalid External Authentication.");
                    }

                    await _userManager.AddToRoleAsync(user, UserRole.User.ToString().ToLower());
                }
                
                await _userManager.AddLoginAsync(user, info);
            }
            
            if (user is null)
                throw new ArgumentException("Invalid External Authentication.");

            var token = await _tokenService.CreateTokenAsync(user);
            return new AuthorizationResponse() { Nickname = user.Nickname, Token = token };
        }
    }
}
