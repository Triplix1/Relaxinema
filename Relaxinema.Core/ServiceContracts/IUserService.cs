using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;

namespace Relaxinema.Core.ServiceContracts
{
    public interface IUserService
    {
        Task<UserResponse?> GetByIdAsync(Guid id);
        Task<UserResponse?> GetByNicknameAsync(string nickname);
        Task<UserResponse?> GetByEmailAsync(string email);
        Task<PagedList<UserResponse>> GetAllAsync(PaginatedParams pagination,bool? admins);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<string>> GetSubscribedEmailsByFilm(Guid filmId);
        Task<AccountInfoResponse> GetAccountInfo(Guid userId);
    }
}
