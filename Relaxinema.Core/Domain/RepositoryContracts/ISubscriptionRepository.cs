using Relaxinema.Core.Domain.Entities;

namespace Relaxinema.Core.Domain.RepositoryContracts;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetSubscriptionAsync(Guid filmId, Guid userId);
    Task CreateSubscriptionAsync(Subscription subscription);
    Task<IEnumerable<Subscription>> GetAllSubscriptionsByFilm(Guid filmId);
    Task<bool> DeleteAsync(Guid filmId, Guid userId);
    Task DeleteByFilm(Guid filmId);
}