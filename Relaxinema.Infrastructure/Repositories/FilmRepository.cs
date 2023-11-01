using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Infrastructure.DatabaseContext;

namespace Relaxinema.Infrastructure.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public FilmRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task CreateAsync(Film entity)
        {
            await _context.Films.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var film = await _context.Films.FirstOrDefaultAsync(x => x.Id == id);
            
            if(film == null)
                return false;

            _context.Films.Remove(film);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PagedList<Film>> GetAllAsync(FilmParams filmParams, string[]? includeStrings = null)
        {
            return await ApplyFilters(filmParams, includeStrings);
        }

        public async Task<Film?> GetByIdAsync(Guid id, string[]? includeStrings = null)
        {
            var query = _context.Films.AsQueryable(); 

            if (includeStrings is not null)
            {
                foreach (var str in includeStrings)
                    query = query.Include(str);
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Film?> UpdateAsync(Film entity)
        {
            var film = await _context.Films.Include(f => f.Genres).FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (film == null)
                return null;

            _mapper.Map(entity, film);

            film.Genres.Clear();
            await _context.SaveChangesAsync();

            film.Genres = entity.Genres;
            await _context.SaveChangesAsync();

            return film;
        }

        private async Task<PagedList<Film>> ApplyFilters(FilmParams filmParams, string[]? includeStrings)
        {
            var query = _context.Films.AsQueryable();

            if(includeStrings is not null)
            {
                foreach(var str in includeStrings)
                    query = query.Include(str);
            }                

            if (filmParams.Year is not null)
                query = query.Where(f => f.Year == filmParams.Year);

            if(filmParams.Genre is not null)
                query = query.Where(f => f.Genres.Any(g => g.Name == filmParams.Genre));

            if(filmParams.OrderByParams is not null)
            {
                var currentOrderBy = filmParams.OrderByParams;

                if (currentOrderBy.Asc)
                {
                    switch (currentOrderBy.OrderBy)
                    {
                        case "Year":
                            query = query.OrderBy(f => f.Year);
                            break;
                        case "Name":
                            query = query.OrderBy(f => f.Name);
                            break;
                        default:
                            query = query.OrderBy(f => f.Name);
                            break;
                    }
                }
                else
                {
                    switch (currentOrderBy.OrderBy)
                    {
                        case "Year":
                            query = query.OrderByDescending(f => f.Year);
                            break;
                        case "Name":
                            query = query.OrderByDescending(f => f.Name);
                            break;
                        default:
                            query = query.OrderByDescending(f => f.Name);
                            break;
                    }
                }
            }

            return await PagedList<Film>.CreateAsync(query, filmParams.PageNumber, filmParams.PageSize);
        }
    }
}
