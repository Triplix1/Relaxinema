using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Helpers.RepositoryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.Domain.RepositoryContracts
{
    public interface IRoleRepository
    {
        Task<Role?> GetByNameAsync(string name, RoleParams? roleParams = null);
        Task<bool> AddToRoleAsync(User user, Role role);
        Task<Role?> GetByIdAsync(Guid id, RoleParams? roleParams = null);
        Task<IEnumerable<Role>> GetAllAsync(RoleParams? roleParams = null);
        Task CreateAsync(Role entity);
        Task<Role?> UpdateAsync(Role entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
