using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BLL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGames([FromQuery] string? genre, [FromQuery] string? name)
        {
            var resultGames = await _gameService.GetAllAsync(genre, name);
            return Ok(resultGames);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGameById(string id)
        {
            GameModel resultGames = await _gameService.GetByIdAsync(Guid.Parse(id));
            return Ok(resultGames);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateGame([FromBody] GameModel model)
        {
            var newGame = await _gameService.AddAsync(model);
            return CreatedAtAction(nameof(GetGameById), new {id = model.Id},  newGame);
        }

        [HttpPut]
        [Authorize(Roles = "Manager, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateGame([FromBody] GameModel model)
        {
            var updatedGame = await _gameService.UpdateAsync(model);
            return Ok(updatedGame);
        }

        [HttpDelete]
        [Authorize(Roles = "Manager, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteGame([FromBody] GameModel model)
        {
            await _gameService.DeleteAsync(model);
            return Ok("Deleted successfully");
        }

       
    }
}
