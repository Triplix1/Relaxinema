using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.ServiceContracts
{
    public interface IUserService
    {
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByNicknameAsync(string nickname);
        Task<IEnumerable<User>> GetAllAsync();
        Task CreateUser(RegisterDto userDto);
        Task DeleteAsync(Guid id);
    }
}
