using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Infrastructure.DatabaseContext;

namespace Relaxinema.Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GenreRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _context = applicationDbContext;
            _mapper = mapper;
        }
        public async Task CreateGenreAsync(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteGenreAsync(Guid id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);

            if (genre == null)
                return false;

            _context.Genres.Remove(genre);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<PagedList<Genre>> GetAllAsync(GenreParams genreParams)
        {
            return await PagedList<Genre>.CreateAsync(_context.Genres.AsQueryable(), genreParams.PageNumber, genreParams.PageSize);
        }

        public async Task<Genre?> GetByIdAsync(Guid id)
        {
            return await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Genre?> GetByNameAsync(string name)
        {
            return await _context.Genres.FirstOrDefaultAsync(g => g.Name == name);
        }

        public async Task<Genre?> UpdateGenreAsync(Genre genre)
        {
            var original = await _context.Genres.FirstOrDefaultAsync(g => g.Id == genre.Id);

            if (original == null) 
                return null;

            _mapper.Map(genre, original);

            await _context.SaveChangesAsync();

            return original;
        }
    }
}
