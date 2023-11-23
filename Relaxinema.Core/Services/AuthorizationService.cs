using AutoMapper;
using Relaxinema.Core.Exceptions;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Core.ServiceContracts;
using System.Security.Cryptography;
using System.Text;
using Relaxinema.Core.DTO.User;
using Relaxinema.Core.DTO.Authorization;

namespace Relaxinema.Core.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AuthorizationService(IUserRepository userRepository, ITokenService tokenService, IMapper mapper, IRoleService roleService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _roleService = roleService;
        }
        public async Task<AuthorizationResponse> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email, new[] { nameof(User.Roles) });

            if (user == null) throw new AuthorizationException("Invalid Username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) throw new AuthorizationException("Invalid Password");
            }

            return new AuthorizationResponse
            {
                Nickname = user.Nickname,
                Token = _tokenService.CreateToken(user)
            };
        }

        public async Task<AuthorizationResponse> RegisterUserAsync(RegisterDto registerDto)
        {
            var userWithSameNickname = await _userRepository.GetByNicknameAsync(registerDto.Nickname);

            if (userWithSameNickname != null)
            {
                throw new AuthorizationException("Here is already user with the same nickname");
            }

            var userWithSameEmail = await _userRepository.GetByEmailAsync(registerDto.Email);

            if (userWithSameEmail != null)
            {
                throw new AuthorizationException("Here is already user with the same email");
            }

            var user = _mapper.Map<User>(registerDto);

            using var hmac = new HMACSHA512();

            user.Nickname = registerDto.Nickname.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
            await _userRepository.CreateAsync(user);

            await _roleService.AddToRoleAsync(user.Id, "User");   

            return new AuthorizationResponse
            {
                Nickname = user.Nickname,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
