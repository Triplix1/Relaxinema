using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.Domain.RepositoryContracts
{
    public interface IUserRepository
    {
        Task<User?> GetByNicknameAsync(string nickname, string[]? includeProperties = null);
        Task<PagedList<User>> GetAllAsync(UserParams userParams, string[] includeProperties);
        Task<User?> GetByEmailAsync(string email, string[]? includeProperties = null);
        Task<IEnumerable<string>?> GetEmailsByFilmAsync(Guid filmId);
        Task<User?> GetByIdAsync(Guid id, string[]? includeProperties = null);
        Task CreateAsync(User entity);
        Task<bool> DeleteAsync(Guid id);
        Task<User> UpdateAsync(User userToUpdate);
    }
}
