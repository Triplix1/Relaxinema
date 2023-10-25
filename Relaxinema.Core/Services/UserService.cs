using AutoMapper;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.DTO;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.Services
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task CreateUser(RegisterDto userDto)
        {
            ValidationHelper.ModelValidation(userDto);

            var userWithSameNickname = await _userRepository.GetByNicknameAsync(userDto.Nickname);

            if(userWithSameNickname != null)
            {
                throw new ArgumentException("Here is already user with the same nickname");
            }

            var userWithSameEmail = await _userRepository.GetByEmailAsync(userDto.Email);

            if (userWithSameEmail != null)
            {
                throw new ArgumentException("Here is already user with the same email");
            }

            var user = _mapper.Map<User>(userWithSameNickname);

            await _userRepository.CreateAsync(user);
        }

        public async Task DeleteAsync(Guid id)
        {
            if(!await _userRepository.DeleteAsync(id))
                throw new KeyNotFoundException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            return _userRepository.GetAllAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new KeyNotFoundException();

            return user;
        }

        public async Task<User> GetByNicknameAsync(string nickname)
        {
            var user = await _userRepository.GetByNicknameAsync(nickname);

            if (user == null)
                throw new KeyNotFoundException();

            return user;
        }
    }
}
