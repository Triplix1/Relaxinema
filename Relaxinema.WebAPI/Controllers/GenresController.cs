using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Relaxinema.Core.DTO.Film;
using Relaxinema.Core.DTO.Genre;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Core.ServiceContracts;
using Relaxinema.WebAPI.Controllers.Base;

namespace Relaxinema.WebAPI.Controllers
{
    public class GenresController : BaseController
    {
        private readonly IGenreService _genreService;
        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenreResponse>> GetById([FromRoute]Guid id)
        {
            return Ok(await _genreService.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<GenreResponse>>> GetAll([FromQuery]GenreParams genreParams)
        {
            return Ok(await _genreService.GetAllPaginatedAsync(genreParams));
        }

        [HttpGet("names")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllGenresNames()
        {
            return Ok(await _genreService.GetAllNamesAsync());
        }
        
        [HttpPost("create")]
        public async Task<ActionResult<GenreResponse>> Create([FromBody]GenreAddRequest genreAddRequest)
        {
            return Ok(await _genreService.CreateGenreAsync(genreAddRequest));
        }

        [HttpPut("edit")]
        public async Task<ActionResult<GenreResponse>> Edit([FromBody]GenreUpdateRequest genreUpdateRequest)
        {
            return Ok(await _genreService.UpdateGenreAsync(genreUpdateRequest));
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete([FromRoute]Guid id)
        {
            await _genreService.DeleteGenreAsync(id);
            return Ok();
        }

    }
}
