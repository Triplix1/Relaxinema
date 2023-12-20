using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Relaxinema.Core.DTO.Rating;
using Relaxinema.Core.Extentions;
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

    [Authorize]
    [HttpGet("user-rate/{filmId}")]
    public async Task<ActionResult<RatingResponse>> GetUserRate([FromRoute] Guid filmId)
    {
        var userId = User.GetUserId();
        
        return Ok(await _ratingService.GetUserRateAsync(filmId, userId));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RatingResponse>> RateFilm([FromBody] RatingRequest ratingRequest)
    {
        var userId = User.GetUserId();
        
        return Ok(await _ratingService.RateFilmAsync(ratingRequest, userId));
    }
}