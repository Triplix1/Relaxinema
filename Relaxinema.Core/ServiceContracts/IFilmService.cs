using Relaxinema.Core.DTO.Film;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;

namespace Relaxinema.Core.ServiceContracts
{
    public interface IFilmService
    {
        Task<FilmResponse> GetByIdAsync(Guid id);
        Task<PagedList<FilmCardResponse>> GetAllAsync(FilmParams filmParams);
        Task<FilmResponse> CreateFilmAsync(FilmAddRequest filmAddRequest);
        Task DeleteAsync(Guid id);
        Task<FilmResponse> UpdateFilmAsync(FilmUpdateRequest filmUpdateRequest);
        Task<IEnumerable<TrailerResponse>> GetFilmTrailers(int n);
        Task<IEnumerable<short>> GetYears();
    }
}
