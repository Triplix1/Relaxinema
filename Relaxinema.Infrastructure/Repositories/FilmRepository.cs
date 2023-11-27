using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Infrastructure.DatabaseContext;
using Relaxinema.Infrastructure.RepositoryHelpers;

namespace Relaxinema.Infrastructure.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IRatingRepository _ratingRepository;
        public FilmRepository(ApplicationDbContext context, IMapper mapper, IRatingRepository ratingRepository)
        {
            _context = context;
            _mapper = mapper;
            _ratingRepository = ratingRepository;
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

        public async Task<IEnumerable<Trailer>> GetTrailers(int n)
        {
            return await _context.Films
                .Where(f => f.IsExpected)
                .Take(n)
                .Select(f => new Trailer
                {
                    Id = f.Id,
                    Name = f.Name,
                    Frame = f.Trailer
                 }).ToArrayAsync();
        }

        public async Task<IEnumerable<short>> GetFilmYears()
        {
            return await _context.Films.Where(f => f.Year.HasValue).Select(f => f.Year.Value).Distinct().ToArrayAsync();
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

            query = IncludeParamsHelper<Film>.IncludeStrings(includeStrings, query);  
            
            if(filmParams.FilterParams is null)
                return await PagedList<Film>.CreateAsync(query, filmParams.PageNumber, filmParams.PageSize);

            if (filmParams.Search is not null)
                query = query.Where(f => f.Name.ToLower().Contains(filmParams.Search.ToLower()));

            if (filmParams.FilterParams.Year is not null)
                query = query.Where(f => f.Year == filmParams.FilterParams.Year);

            if(filmParams.FilterParams.Genre is not null)
                query = query.Where(f => f.Genres.Any(g => g.Name == filmParams.FilterParams.Genre));

            if (filmParams.FilterParams.Expected is not null)
                query = query.Where(f => f.IsExpected == filmParams.FilterParams.Expected);

            if (!filmParams.ShowHiddens)
                query = query.Where(f => f.Publish);

            if(filmParams.OrderByParams is { OrderBy: not null, Asc: not null })
            {
                var currentOrderBy = filmParams.OrderByParams;

                if (currentOrderBy.Asc.Value)
                {
                    switch (currentOrderBy.OrderBy)
                    {
                        case "Рік":
                            query = query.OrderBy(f => f.Year ?? short.MaxValue);
                            break;
                        case "Назва":
                            query = query.OrderBy(f => f.Name);
                            break;
                        case "Рейтинг":
                            query = query.OrderBy(f => f.Ratings.Count == 0 ? 0 : f.Ratings.Sum(r => r.Rate) / f.Ratings.Count);
                            break;
                    }
                }
                else
                {
                    switch (currentOrderBy.OrderBy)
                    {
                        case "Рік":
                            query = query.OrderByDescending(f => f.Year ?? short.MaxValue);
                            break;
                        case "Назва":
                            query = query.OrderByDescending(f => f.Name);
                            break;
                        case "Рейтинг":
                            query = query.OrderByDescending(f => f.Ratings.Count == 0 ? 0 : f.Ratings.Sum(r => r.Rate) / f.Ratings.Count);
                            break;
                    }
                }
            }

            return await PagedList<Film>.CreateAsync(query, filmParams.PageNumber, filmParams.PageSize);
        }

        
    }
}
