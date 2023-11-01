using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;

namespace Relaxinema.Core.Domain.RepositoryContracts
{
    public interface IFilmRepository
    {
        Task<PagedList<Film>> GetAllAsync(FilmParams filmParams, string[]? includeStrings = null);
        Task<Film?> GetByIdAsync(Guid id, string[]? includeStrings = null);
        Task CreateAsync(Film entity);
        Task<Film?> UpdateAsync(Film entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
