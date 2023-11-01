using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Infrastructure.DatabaseContext;

namespace Relaxinema.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RoleRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddToRoleAsync(User user, Role role)
        {
            user.Roles.Add(role);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task CreateAsync(Role entity)
        {
            _context.Roles.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
                return false;

            _context.Roles.Remove(role);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Role>> GetAllAsync(RoleParams? roleParams = null)
        {
            var query = GetIncludedParams(roleParams);
            return await query.ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(Guid id, RoleParams? roleParams = null)
        {
            var query = GetIncludedParams(roleParams);

            return await query.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Role?> GetByNameAsync(string name, RoleParams? roleParams = null)
        {
            var query = GetIncludedParams(roleParams);

            return await query.FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<Role?> UpdateAsync(Role entity)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == entity.Id);

            if (role == null)
                return null;

            _mapper.Map(entity, role);

            await _context.SaveChangesAsync();
            return role;
        }
        
        private IQueryable<Role> GetIncludedParams(RoleParams? roleParams)
        {
            var query = _context.Roles;

            if (roleParams != null && roleParams.IncludeStrings != null)
            {
                foreach (var item in roleParams.IncludeStrings)
                    query.Include(item);
            }

            return query;
        }
    }
}
