using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Infrastructure.DatabaseContext;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Relaxinema.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(User entity)
        {
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return false;

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<User>> GetAllAsync(UserParams? userParams = null)
        {
            var query = ApplyParams(userParams);

            return await query.ToListAsync();
        }

        public async Task<User?> GetByNicknameAsync(string nickname, UserParams? userParams = null)
        {
            var query = ApplyParams(userParams);

            return await query.FirstOrDefaultAsync(u => u.Nickname == nickname);
        }

        public async Task<User?> GetByIdAsync(Guid id, UserParams? userParams = null)
        {
            var query = ApplyParams(userParams);

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> UpdateAsync(User entity)
        {
            var user = await _context.Users.FindAsync(entity.Id);

            if (user == null)
                return null;

            _mapper.Map(entity, user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> GetByEmailAsync(string email, UserParams? userParams = null)
        {
            var query = ApplyParams(userParams);
            return await query.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<string>?> GetEmailsByFilmAsync(Guid filmId)
        {
            var film = await _context.Films.Include(f => f.SubscribedUsers).FirstOrDefaultAsync(f => f.Id == filmId);

            if (film == null)
                return null;

            return film.SubscribedUsers.Select(u => u.Email);
        }

        private IQueryable<User> ApplyParams(UserParams? userParams)
        {
            var query = _context.Users.AsQueryable();

            if (userParams == null)
                return query;

            if (userParams.IncludeProperties != null)
            {
                foreach (var property in userParams.IncludeProperties)
                    query = query.Include(property);
            }

            if (userParams.Filter != null)
                query = query.Where(userParams.Filter);

            return query;
        }
    }
}
