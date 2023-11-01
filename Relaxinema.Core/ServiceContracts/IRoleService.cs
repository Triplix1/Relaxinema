using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.ServiceContracts
{
    public interface IRoleService
    {
        Task AddToRoleAsync(Guid userId, string roleName);
        Task CreateAsync(Role role);
        Task DeleteAsync(Guid roleId);
        Task<IEnumerable<RoleResponse>> GetAllAsync();
        Task<RoleResponse> GetByIdAsync(Guid id);
        Task<RoleResponse> UpdateAsync(Role entity);
    }
}
