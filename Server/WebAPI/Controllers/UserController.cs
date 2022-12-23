using System.Security.Claims;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserDetails()
        {
            var userId = User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _userService.GetByIdAsync(Guid.Parse(userId));
            return Ok(user);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetUserDetailsById([FromRoute] Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }
    }
}
