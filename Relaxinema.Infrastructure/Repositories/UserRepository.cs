using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Infrastructure.DatabaseContext;

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

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllWithDetailsAsync()
        {
            return await _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Comments)
                .Include(u => u.SubscribedTo)
                .ToListAsync();
        }

        public async Task<User?> GetByNicknameAsync(string nickname)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Nickname == nickname);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> UpdateAsync(User entity)
        {
            var user = await _context.Users.FindAsync(entity.Id);

            if(user == null)
                return null;

           _mapper.Map(entity, user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
