using Relaxinema.Core.Domain.Entities;

namespace Relaxinema.Core.ServiceContracts;

public interface ITokenService
{
    string CreateToken(User user);
}
