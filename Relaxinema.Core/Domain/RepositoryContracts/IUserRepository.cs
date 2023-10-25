using Relaxinema.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.Domain.RepositoryContracts
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByNicknameAsync(string nickname);
        Task<User?> GetByEmailAsync(string email);
    }
}
