using BLL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> GetGames()
        {
            IEnumerable<GameModel> resultGames;
            try
            {
                resultGames = await _gameService.GetAllAsync();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }

            return Ok(resultGames);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetGameById(string id)
        {
            GameModel resultGames;
            try
            {
                resultGames = await _gameService.GetByIdAsync(Guid.Parse(id));
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }

            return Ok(resultGames);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateGame([FromBody] GameModel model)
        {
            GameModel newGame;
            try
            { 
                newGame = await _gameService.AddAsync(model);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok(newGame);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateGame([FromBody] GameModel model)
        {
            try
            {
                await _gameService.UpdateAsync(model);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok("Updated successfully");
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteGame([FromBody] GameModel model)
        {
            try
            {
                _gameService.Delete(model);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok("Deleted successfully");
        }
    }
}
