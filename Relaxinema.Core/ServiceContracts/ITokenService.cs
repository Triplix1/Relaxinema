using Relaxinema.Core.Domain.Entities;

namespace Relaxinema.Core.ServiceContracts;

public interface ITokenService
{
    Task<string> CreateTokenAsync(User user);
}
