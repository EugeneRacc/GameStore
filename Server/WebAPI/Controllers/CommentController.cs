using BLL.Interfaces;
using BLL.Models;
using BLL.Services;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<IActionResult> GetGameComments([FromRoute] Guid id)
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
            var updatedComment = await _commentService.UpdateAsync(model);
            return Ok(updatedComment);
        }

        [HttpDelete]
        [Authorize(Roles = "User, Manager, Admin")]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteComment([FromBody] CommentModel model,
            [FromRoute] Guid id)
        {
            if (((!User.IsInRole("Manager") || (!User.IsInRole("Admin"))
                    && model.UserId.ToString() ==
                    User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value))
                || (User.IsInRole("Admin") || User.IsInRole("Manager")))
            {
                model.GameId = id;
                await _commentService.DeleteAsync(model);
                return Ok(new { Response = "Deleted" });
            }
            return BadRequest(new { Response = "Do not have such permissions" });
        }

        [HttpDelete]
        [Authorize(Roles = "User, Manager, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteComments([FromBody] CommentCollectionModel models)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Manager"))
            {
                await _commentService.DeleteAsync(models.Comments);
            }
            else
            {
                await _commentService.DeleteAsync(models.Comments, User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value);
            }
            return Ok(new { Response = "Deleted" });
        }
    }
}
