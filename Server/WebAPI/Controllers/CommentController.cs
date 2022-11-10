using BLL.Interfaces;
using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGames([FromRoute] Guid id)
        {
            var resultComment = await _commentService.GetGameComments(id);
            return Ok(resultComment);
        }

        [HttpPost]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddComment([FromBody] CommentModel model, [FromRoute] Guid id)
        {
            model.GameId = id;
            var addedComment = await _commentService.AddAsync(model);
            return Ok(addedComment);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateComment([FromBody] CommentModel model, [FromRoute] Guid id)
        {
            model.GameId = id;
            var addedComment = await _commentService.UpdateAsync(model);
            return Ok(addedComment);
        }
    }
}
