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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Relaxinema.Core.Helpers.RepositoryParams;

namespace Relaxinema.Core.Services
{
    internal class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IFilmRepository _filmRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UserService(UserManager<User> userManager, IFilmRepository filmRepository, IMapper mapper, IUserRepository userRepository, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _filmRepository = filmRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _roleManager = roleManager;
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is not null && !(await _userManager.DeleteAsync(user)).Succeeded)
                throw new KeyNotFoundException();
        }

        public async Task<PagedList<UserResponse>> GetAllAsync(PaginatedParams pagination, bool? admins)
        {
            UserParams userParams = new UserParams()
            {
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                Admins = admins,
            };

            var result = await _userRepository.GetAllAsync(userParams, new [] {"Roles"});

            return new PagedList<UserResponse>(_mapper.Map<IEnumerable<UserResponse>>(result.Items), result.TotalCount,
                result.CurrentPage, result.PageSize);
        }

        public async Task<UserResponse?> GetByEmailAsync(string email)
        {
            return _mapper.Map<UserResponse>(await _userManager.FindByEmailAsync(email));
        }

        public async Task<UserResponse?> GetByIdAsync(Guid id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse?> GetByNicknameAsync(string nickname)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Nickname == nickname);

            if (user == null)
                throw new KeyNotFoundException();

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<IEnumerable<string>> GetSubscribedEmailsByFilm(Guid filmId)
        {
            var emails = await _userRepository.GetEmailsByFilmAsync(filmId);

            if (emails == null)
                throw new KeyNotFoundException("doesn't contains film with such id");

            return emails;
        }
    }
}