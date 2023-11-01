using AutoMapper;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.DTO.User;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.Services
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFilmRepository _filmRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IFilmRepository filmRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _filmRepository = filmRepository;
            _mapper = mapper;
        }

        public async Task DeleteAsync(Guid id)
        {
            if(!await _userRepository.DeleteAsync(id))
                throw new KeyNotFoundException();
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<UserDto>>(await _userRepository.GetAllAsync());
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            return _mapper.Map<UserDto>(await _userRepository.GetByEmailAsync(email));
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetByNicknameAsync(string nickname)
        {
            var user = await _userRepository.GetByNicknameAsync(nickname);

            if (user == null)
                throw new KeyNotFoundException();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<string>> GetSubscribedEmailsByFilm(Guid filmId)
        {
            var emails = await _userRepository.GetEmailsByFilmAsync(filmId);

            if(emails == null)
                throw new KeyNotFoundException("doesn't contains film with such id");

            return emails;
        }
    }
}
