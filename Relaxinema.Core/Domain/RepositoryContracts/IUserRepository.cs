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
        Task<User?> GetByNicknameAsync(string nickname, UserParams? userParams = null);
        Task<IEnumerable<User>> GetAllAsync(UserParams? userParams = null);
        Task<User?> GetByEmailAsync(string email, UserParams? userParams = null);
        Task<IEnumerable<string>?> GetEmailsByFilmAsync(Guid filmId);
        Task<User?> GetByIdAsync(Guid id, UserParams? userParams = null);
        Task CreateAsync(User entity);
        Task<User?> UpdateAsync(User entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
