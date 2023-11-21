using Relaxinema.Core.DTO.Subscribe;

namespace Relaxinema.Core.ServiceContracts;

public interface ISubscriptionService
{
    Task<SubscribeResponse?> GetSubscriptionAsync(Guid filmId, Guid userId);
    Task<SubscribeResponse> CreateSubscriptionAsync(SubscribeAddRequest subscription, Guid userId);
    Task<IEnumerable<SubscribeResponse>> GetAllSubscriptionsByFilm(Guid filmId);
    Task DeleteAsync(Guid filmId, Guid userId);
}