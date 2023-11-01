using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.Domain.RepositoryContracts
{
    public interface IGenreRepository
    {
        Task<Genre?> GetByIdAsync(Guid id);
        Task<Genre?> GetByNameAsync(string name);
        Task<PagedList<Genre>> GetAllAsync(GenreParams genreParams);
        Task CreateGenreAsync(Genre genre);
        Task<Genre?> UpdateGenreAsync(Genre genre);
        Task<bool> DeleteGenreAsync(Guid id);
    }
}
