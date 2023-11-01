using AutoMapper;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.DTO.Genre;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        public GenreService(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }
        public async Task<GenreResponse> CreateGenreAsync(GenreAddRequest genreAddRequest)
        {
            ValidationHelper.ModelValidation(genreAddRequest);

            var genre = await _genreRepository.GetByNameAsync(genreAddRequest.Name);

            if (genre is not null)
                throw new ArgumentException("Already exists genre with such name");

            var genreResult = _mapper.Map<Genre>(genreAddRequest);

            await _genreRepository.CreateGenreAsync(genreResult);

            return _mapper.Map<GenreResponse>(genreResult);
        }

        public async Task DeleteGenreAsync(Guid id)
        {
            if (!await _genreRepository.DeleteGenreAsync(id))
                throw new KeyNotFoundException("Cannot find genre with such id");
        }

        public async Task<PagedList<GenreResponse>> GetAllAsync(GenreParams genreParams)
        {
            var response = await _genreRepository.GetAllAsync(genreParams);

            return new PagedList<GenreResponse>(
                _mapper.Map<IEnumerable<GenreResponse>>(response),
                response.TotalCount,
                response.CurrentPage,
                response.PageSize
            );
        }

        public async Task<GenreResponse> GetByIdAsync(Guid id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);

            if(genre is null)
                throw new KeyNotFoundException("Cannot find genre with such id");

            return _mapper.Map<GenreResponse>(genre);
        }

        public async Task<GenreResponse> GetByNameAsync(string name)
        {
            var genre = await _genreRepository.GetByNameAsync(name);

            if (genre is null)
                throw new KeyNotFoundException("Cannot find genre with such name");

            return _mapper.Map<GenreResponse>(genre);
        }

        public async Task<GenreResponse> UpdateGenreAsync(GenreUpdateRequest genreUpdateRequest)
        {
            ValidationHelper.ModelValidation(genreUpdateRequest);

            var genre = _mapper.Map<Genre>(genreUpdateRequest); 

            var result = await _genreRepository.UpdateGenreAsync(genre);

            if (result is null)
                throw new KeyNotFoundException("Cannot find genre with such id");

            return _mapper.Map<GenreResponse>(result);
        }
    }
}
