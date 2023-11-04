using Microsoft.AspNetCore.Mvc;
using Relaxinema.Core.DTO.Rating;
using Relaxinema.Core.ServiceContracts;
using Relaxinema.WebAPI.Controllers.Base;

namespace Relaxinema.WebAPI.Controllers;

public class RatingsController : BaseController
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [HttpGet("{filmId}")]
    public async Task<ActionResult<float>> GetFilmRating([FromRoute] Guid filmId)
    {
        return Ok(await _ratingService.GetRatingAsync(filmId));
    }

    [HttpGet("{filmId}/{userId}")]
    public async Task<ActionResult<RatingResponse>> GetUserRate([FromRoute] Guid filmId, [FromRoute] Guid userId)
    {
        return Ok(await _ratingService.GetUserRateAsync(filmId, userId));
    }

    [HttpPost("rate")]
    public async Task<ActionResult<RatingResponse>> RateFilm([FromBody] RatingRequest ratingRequest)
    {
        return Ok(await _ratingService.RateFilmAsync(ratingRequest));
    }
}