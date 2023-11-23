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
using Relaxinema.Core.Helpers.RepositoryParams;

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

        public async Task<PagedList<UserResponse>> GetAllAsync(PaginatedParams pagination, bool? admins)
        {
            UserParams? userParams = null;

            if (admins is not null)
            {
                userParams = new UserParams()
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    Admins = admins
                };
            }

            var result = await _userRepository.GetAllAsync(userParams, new[] { nameof(User.Roles) });
            
            return new PagedList<UserResponse>(_mapper.Map<IEnumerable<UserResponse>>(result.Items), result.TotalCount, result.CurrentPage, result.PageSize);
        }

        public async Task<UserResponse?> GetByEmailAsync(string email)
        {
            return _mapper.Map<UserResponse>(await _userRepository.GetByEmailAsync(email));
        }

        public async Task<UserResponse?> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse?> GetByNicknameAsync(string nickname)
        {
            var user = await _userRepository.GetByNicknameAsync(nickname);

            if (user == null)
                throw new KeyNotFoundException();

            return _mapper.Map<UserResponse>(user);
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
