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
        public FilmService(IFilmRepository filmRepository, IMapper mapper, IGenreRepository genreRepository)
        {
            _filmRepository = filmRepository;
            _mapper = mapper;
            _genreRepository = genreRepository;
        }
        public async Task<FilmResponse> CreateFilmAsync(FilmAddRequest filmAddRequest)
        {
            ValidationHelper.ModelValidation(filmAddRequest);

            var film = _mapper.Map<Film>(filmAddRequest);
            film.Created = DateTime.Now;
            film.Genres = new List<Genre>();

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

        public async Task<PagedList<FilmResponse>> GetAllAsync(FilmParams filmParams)
        {
            var pagedList = await _filmRepository.GetAllAsync(filmParams, new[] { nameof(Film.Genres) });

            return new PagedList<FilmResponse>(
                _mapper.Map<IEnumerable<FilmResponse>>(pagedList), 
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
            ValidationHelper.ModelValidation(filmUpdateRequest);

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
    }
}
