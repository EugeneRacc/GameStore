using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGenres([FromQuery] string? gameId)
        {
            if (gameId == null)
                return Ok(await _genreService.GetAllAsync());
            return Ok(await _genreService.GetAllByGameIdAsync(Guid.Parse(gameId)));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGenreById(string id)
        {
            var resultGames = await _genreService.GetGenreByIdAsync(Guid.Parse(id));
            return Ok(resultGames);
        }
    }
}
