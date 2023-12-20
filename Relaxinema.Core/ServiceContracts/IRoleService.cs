using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.DTO;

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
