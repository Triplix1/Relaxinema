using AutoMapper;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.DTO.Film;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Core.MailConfig;
using Relaxinema.Core.ServiceContracts;

namespace Relaxinema.Core.Services
{
    public class FilmService : IFilmService
    {
        private readonly IFilmRepository _filmRepository;
        private readonly IMapper _mapper;
        private readonly IGenreRepository _genreRepository;
        private readonly IPhotoService _photoService;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMailService _mailService;
        private const int PhotoHeight = 750;
        private const int PhotoWidth = 500;
        
        public FilmService(IFilmRepository filmRepository, IMapper mapper, IGenreRepository genreRepository, IPhotoService photoService, ISubscriptionRepository subscriptionRepository, IMailService mailService)
        {
            _filmRepository = filmRepository;
            _mapper = mapper;
            _genreRepository = genreRepository;
            _photoService = photoService;
            _subscriptionRepository = subscriptionRepository;
            _mailService = mailService;
        }
        public async Task<FilmResponse> CreateFilmAsync(FilmAddRequest filmAddRequest)
        {
            if (filmAddRequest.Year == 0)
            {
                filmAddRequest.Year = null;
            }
            
            if (filmAddRequest.Limitation == 0)
            {
                filmAddRequest.Limitation = null;
            }
            
            var film = _mapper.Map<Film>(filmAddRequest);
            film.Created = DateTime.Now;
            film.Genres = new List<Genre>();

            if (filmAddRequest.File is not null)
            {
                var result = await _photoService.AddPhotoAsync(filmAddRequest.File, PhotoHeight, PhotoWidth);

                if (result.Error != null) throw new ArgumentException(result.Error.Message);

                film.PhotoUrl = result.SecureUrl.AbsoluteUri;
                film.PhotoPublicId = result.PublicId;
            }

            foreach (var genre in filmAddRequest.GenreNames.Split(","))
            {
                var foundGenre = await _genreRepository.GetByNameAsync(genre);

                if (foundGenre is null)
                    throw new KeyNotFoundException("Cannot find genre with such id");

                film.Genres.Add(foundGenre);
            }

            await _filmRepository.CreateAsync(film);

            return _mapper.Map<FilmResponse>(film);
        }

        public async Task DeleteAsync(Guid id)
        {
            if(!await _filmRepository.DeleteAsync(id))
                throw new KeyNotFoundException();
            
        }

        public async Task<PagedList<FilmCardResponse>> GetAllAsync(FilmParams filmParams)
        {
            var stringsToInclude = new List<string>();
            stringsToInclude.Add(nameof(Film.Genres));
            
            if (filmParams.OrderByParams?.OrderBy is not null && filmParams.OrderByParams.OrderBy == "Рейтинг")
            {
                stringsToInclude.Add(nameof(Film.Ratings));
            }
            
            var pagedList = await _filmRepository.GetAllAsync(filmParams, stringsToInclude.ToArray());

            return new PagedList<FilmCardResponse>(
                _mapper.Map<IEnumerable<FilmCardResponse>>(pagedList.Items), 
                pagedList.TotalCount, 
                pagedList.CurrentPage, 
                pagedList.PageSize
            );
        }

        public async Task<FilmResponse> GetByIdAsync(Guid id)
        {
            var film = await _filmRepository.GetByIdAsync(id, new[] { nameof(Film.Comments), nameof(Film.Genres) });

            if(film is null)
                throw new KeyNotFoundException();

            return _mapper.Map<FilmResponse>(film);
        }

        public async Task<FilmResponse> UpdateFilmAsync(FilmUpdateRequest filmUpdateRequest)
        {
            Film? oldFilm;
            if(filmUpdateRequest.Id.HasValue)
                oldFilm = await _filmRepository.GetByIdAsync(filmUpdateRequest.Id.Value);
            else
                throw new KeyNotFoundException();

            if (oldFilm is null)
                throw new KeyNotFoundException();
            
            if (filmUpdateRequest.Year == 0)
            {
                filmUpdateRequest.Year = null;
            }
            
            if (filmUpdateRequest.Limitation == 0)
            {
                filmUpdateRequest.Limitation = null;
            }
            
            var film = _mapper.Map<Film>(filmUpdateRequest);
            film.Genres = new List<Genre>();

            if (filmUpdateRequest.File is null)
            {
                film.PhotoPublicId = oldFilm.PhotoPublicId;
                film.PhotoUrl = oldFilm.PhotoUrl;
            }
            else
            {
                var result = await _photoService.AddPhotoAsync(filmUpdateRequest.File, PhotoHeight, PhotoWidth);

                if (result.Error != null) throw new ArgumentException(result.Error.Message);
                
                if(oldFilm.PhotoPublicId is not null)
                    await _photoService.DeletePhotoAsync(oldFilm.PhotoPublicId);

                film.PhotoUrl = result.SecureUrl.AbsoluteUri;
                film.PhotoPublicId = result.PublicId;
            }
            
            foreach (var genre in filmUpdateRequest.GenreNames.Split(","))
            {
                var foundGenre = await _genreRepository.GetByNameAsync(genre);

                if (foundGenre is not null)
                    film.Genres.Add(foundGenre);
            }

            if (oldFilm.IsExpected && !film.IsExpected)
            {
                var message = new MessageRequest()
                {
                    To = (await _subscriptionRepository.GetAllSubscriptionsByFilm(oldFilm.Id)).Select(s => s.User.Email)
                        .ToList(),
                    Subject = "Film has been upload",
                    Content = $@"<img src=""{film.PhotoUrl}"" style=""width:200px"">"
                };
                await _mailService.SendHtmlAsync(message);
                await _subscriptionRepository.DeleteByFilm(oldFilm.Id);
            }

            var updatedFilm = await _filmRepository.UpdateAsync(film);

            if(updatedFilm is null)
                throw new KeyNotFoundException();

            return _mapper.Map<FilmResponse>(updatedFilm);
        }

        public async Task<IEnumerable<TrailerResponse>> GetFilmTrailers(int n)
        {
            return _mapper.Map<IEnumerable<TrailerResponse>>(await _filmRepository.GetTrailers(n));
        }

        public async Task<IEnumerable<short>> GetYears()
        {
            return await _filmRepository.GetFilmYears();
        }
    }
}
