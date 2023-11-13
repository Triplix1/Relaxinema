using AutoMapper;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.DTO.Film;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Core.ServiceContracts;

namespace Relaxinema.Core.Services
{
    public class FilmService : IFilmService
    {
        private readonly IFilmRepository _filmRepository;
        private readonly IMapper _mapper;
        private readonly IGenreRepository _genreRepository;
        private readonly IPhotoService _photoService;
        public FilmService(IFilmRepository filmRepository, IMapper mapper, IGenreRepository genreRepository, IPhotoService photoService)
        {
            _filmRepository = filmRepository;
            _mapper = mapper;
            _genreRepository = genreRepository;
            _photoService = photoService;
        }
        public async Task<FilmResponse> CreateFilmAsync(FilmAddRequest filmAddRequest)
        {
            var film = _mapper.Map<Film>(filmAddRequest);
            film.Created = DateTime.Now;
            film.Genres = new List<Genre>();

            if (filmAddRequest.File is not null)
            {
                var result = await _photoService.AddPhotoAsync(filmAddRequest.File);

                if (result.Error != null) throw new ArgumentException(result.Error.Message);

                film.PhotoUrl = result.SecureUrl.AbsoluteUri;
                film.PhotoPublicId = result.PublicId;
            }

            foreach (var genre in filmAddRequest.GenreNames)
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
            var pagedList = await _filmRepository.GetAllAsync(filmParams, new[] { nameof(Film.Genres) });

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
            var film = _mapper.Map<Film>(filmUpdateRequest);
            film.Genres = new List<Genre>();

            foreach (var genre in filmUpdateRequest.GenreNames)
            {
                var foundGenre = await _genreRepository.GetByNameAsync(genre);

                if (foundGenre is not null)
                    film.Genres.Add(foundGenre);
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
