using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Infrastructure.DatabaseContext;
using System.Linq.Expressions;
using Relaxinema.Infrastructure.RepositoryHelpers;
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

        public async Task<User> UpdateAsync(User user)
        {
            var original = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (user is null)
                return null;

            original.Nickname = user.Nickname;
            original.PhotoUrl = user.PhotoUrl;
            original.PhotoPublicId = user.PhotoPublicId;
            await _context.SaveChangesAsync();

            return original;
        }

        public async Task<PagedList<User>> GetAllAsync(UserParams userParams, string[]? includeProperties)
        {
            return await ApplyParams(userParams, includeProperties);
        }

        public async Task<User?> GetByNicknameAsync(string nickname, string[]? includeProperties)
        {
            var query = IncludeProps(_context.Users.AsQueryable(), includeProperties);
            return await query.FirstOrDefaultAsync(u => u.Nickname == nickname);
        }

        public async Task<User?> GetByIdAsync(Guid id, string[]? includeProperties)
        {
            var query = IncludeProps(_context.Users.AsQueryable(), includeProperties);
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email, string[]? includeProperties)
        {
            var query = IncludeProps(_context.Users.AsQueryable(), includeProperties);
            return await query.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<string>?> GetEmailsByFilmAsync(Guid filmId)
        {
            var film = await _context.Films.Include(f => f.SubscribedUsers).FirstOrDefaultAsync(f => f.Id == filmId);

            return film?.SubscribedUsers.Select(u => u.Email);
        }

        private async Task<PagedList<User>> ApplyParams(UserParams userParams, string[]? includeProperties)
        {
            var query = _context.Users.AsQueryable();

            query = IncludeProps(query, includeProperties);

            if (userParams.Admins.HasValue && userParams.Admins.Value)
                query = query.Where(u => u.Roles.Any(r => r.Name == "admin"));

            return await PagedList<User>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
        }

        private IQueryable<User> IncludeProps(IQueryable<User> query, string[]? includeStrings)
        {
            if(includeStrings is not null)
                query = IncludeParamsHelper<User>.IncludeStrings(includeStrings, query);

            return query;
        }
    }
}
