using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.DTO.Genre;

namespace Relaxinema.Core.ServiceContracts
{
    public interface IGenreService
    {
        Task<GenreResponse> GetByIdAsync(Guid id);
        Task<GenreResponse> GetByNameAsync(string name);
        Task<PagedList<GenreResponse>> GetAllAsync(GenreParams genreParams);
        Task<GenreResponse> CreateGenreAsync(GenreAddRequest genreAddRequest);
        Task<GenreResponse> UpdateGenreAsync(GenreUpdateRequest genreUpdateRequest);
        Task DeleteGenreAsync(Guid id);
    }
}
