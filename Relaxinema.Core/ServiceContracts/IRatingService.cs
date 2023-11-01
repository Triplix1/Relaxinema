using Relaxinema.Core.DTO.Rating;

namespace Relaxinema.Core.ServiceContracts;

public interface IRatingService
{
    Task<float> GetRatingAsync(Guid filmId);
    Task<RatingResponse> GetUserRateAsync(Guid filmId, Guid userId);
    Task<RatingResponse> RateFilmAsync(RatingRequest ratingRequest);
}