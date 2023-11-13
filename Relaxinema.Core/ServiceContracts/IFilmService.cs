using Relaxinema.Core.DTO.Film;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Relaxinema.Core.Domain.Entities;

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
