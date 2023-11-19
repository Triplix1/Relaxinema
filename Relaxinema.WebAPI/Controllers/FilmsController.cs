using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Relaxinema.Core.DTO.Film;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Core.ServiceContracts;
using Relaxinema.WebAPI.Controllers.Base;

namespace Relaxinema.WebAPI.Controllers
{
    public class FilmsController : BaseController
    {
        private readonly IFilmService _filmService;

        public FilmsController(IFilmService filmService)
        {
            _filmService = filmService;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<FilmResponse>> GetFilmById([FromRoute]Guid id)
        {
            return Ok(await _filmService.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<FilmResponse>>> GetAllFilms([FromHeader]FilmParams filmParams)
        {
            return Ok(await _filmService.GetAllAsync(filmParams));
        }
        
        [HttpGet("trailers/{n}")]
        public async Task<ActionResult<IEnumerable<TrailerResponse>>> GetExpectedTrailers([FromRoute]int n)
        {
            return Ok(await _filmService.GetFilmTrailers(n));
        }

        [HttpGet("years")]
        public async Task<ActionResult<IEnumerable<short>>> GetYears()
        {
            return Ok(await _filmService.GetYears());
        }

        [HttpPost("create")]
        public async Task<ActionResult<FilmResponse>> Create([FromForm]FilmAddRequest filmAddRequest)
        {
            return Ok(await _filmService.CreateFilmAsync(filmAddRequest));
        }

        [HttpPut("edit")]
        public async Task<ActionResult<FilmResponse>> UpdateFilm([FromForm]FilmUpdateRequest filmUpdateRequest)
        {
            return Ok(await _filmService.UpdateFilmAsync(filmUpdateRequest));
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete([FromRoute]Guid id)
        {
            await _filmService.DeleteAsync(id);
            return Ok();
        }
    }
}
