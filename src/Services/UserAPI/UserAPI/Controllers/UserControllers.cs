using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserAPI.Abstractions;
using UserAPI.Models;
using UserAPI.DTOs;

namespace UserAPI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize]
        [HttpPut("me")]
        public async Task<ActionResult<RedactUserRequest>> EditUserAsync([FromBody] RedactUser user, CancellationToken ct)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid token");
            }

            var result = await _userService.EditUserAsync(userId, user, ct);
            if (result == null)
            {
                return NotFound("User not found");
            }
            return Ok(result);
        }
        //[Authorize(Roles = "Admin")]
        //[HttpDelete("delete/{id}")]
        //public async Task<ActionResult> DeleteUser (Guid id, CancellationToken ct)
        //{
        //    if (!User.IsInRole("Admin"))
        //    {
        //        return Forbid();
        //    }
        //    var result = await _userService.DeleteUser(id, ct);
        //    if(result == false)
        //    {
        //        return NotFound("Incorrect ID");
        //    }
        //    return Ok();
        //}
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<User>> ShowUserInfo(CancellationToken ct)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid token");
            }
            var result = await _userService.GetUserById(userId, ct);
            if (result == null)
            {
                return NotFound("User Not found");
            }
            return Ok(result);
        }
    }
}
