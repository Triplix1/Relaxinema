using AutoMapper;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.DTO.User;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.ServiceContracts;
using Relaxinema.Core.DTO.Film;
using Relaxinema.Core.Helpers.RepositoryParams;

namespace Relaxinema.Core.Services
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFilmRepository _filmRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private const int PhotoHeight = 500;
        private const int PhotoWidth = 500;

        public UserService(IUserRepository userRepository, IFilmRepository filmRepository, IMapper mapper, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _filmRepository = filmRepository;
            _mapper = mapper;
            _photoService = photoService;
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

        public async Task<AccountInfoResponse> GetAccountInfo(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId, new[] { nameof(User.SubscribedTo) });

            if (user is null)
                throw new KeyNotFoundException("Cannot Find user with such id");

            var accountResponse = _mapper.Map<AccountInfoResponse>(user);

            accountResponse.SubscribedTo = _mapper.Map<IEnumerable<FilmCardResponse>>(user.SubscribedTo);

            return accountResponse;
        }

        public async Task<AccountInfoResponse> UpdateAsync(UserUpdateRequest userUpdateRequest)
        {
            var user = await _userRepository.GetByIdAsync(userUpdateRequest.Id);
            
            if (user is null)
                throw new KeyNotFoundException("Cannot find user with such id");

            var userWithTheSameNick = await _userRepository.GetByNicknameAsync(userUpdateRequest.Nickname);
            
            if (userWithTheSameNick is not null && userWithTheSameNick.Id != user.Id)
                throw new ArgumentException("This nick has alrady been taken");
                
            user.Nickname = userUpdateRequest.Nickname;
            
            if (userUpdateRequest.File is not null)
            {
                if (user.PhotoPublicId is not null)
                    await _photoService.DeletePhotoAsync(user.PhotoPublicId);

                var imageResult = await _photoService.AddPhotoAsync(userUpdateRequest.File, PhotoHeight, PhotoWidth);
                user.PhotoPublicId = imageResult.PublicId;
                user.PhotoUrl = imageResult.SecureUrl.AbsoluteUri;
            }

            var response = await _userRepository.UpdateAsync(user);

            return _mapper.Map<AccountInfoResponse>(response);
        }
    }
}
