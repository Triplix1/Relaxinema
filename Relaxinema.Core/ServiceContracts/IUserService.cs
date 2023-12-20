using Relaxinema.Core.DTO.User;
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
        Task<AccountInfoResponse> UpdateAsync(UserUpdateRequest userUpdateRequest);
    }
}
