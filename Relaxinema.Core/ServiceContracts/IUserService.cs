using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.ServiceContracts
{
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(Guid id);
        Task<UserDto?> GetByNicknameAsync(string nickname);
        Task<UserDto?> GetByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task DeleteAsync(Guid id);
        Task<IEnumerable<string>> GetSubscribedEmailsByFilm(Guid filmId);
    }
}
