using Relaxinema.Core.Domain.Entities;

namespace Relaxinema.Core.Domain.RepositoryContracts;

public interface IRatingRepository
{
    Task RateAsync(Rating rating);
    Task<float> GetFilmMarkAsync(Film film);
    Task<Rating?> GetRateAsync(User user, Film film);
}