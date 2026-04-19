using AuthorizationAPI.Abstractions;
using AuthorizationAPI.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthControllers : ControllerBase
    {
        private readonly IAuthService _authservice;
        public AuthControllers(IAuthService authService)
        {
            _authservice = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> RegisterUserAsync ([FromBody]RegRequest request, CancellationToken ct)
        {
            var user = new IdentityUser { Email = request.Email };
            var result = await _authservice.RegisterAsync(request, ct);

            if (result == null)
            {
                return Conflict("Email is already ixists");
            }
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> LogInAsync([FromBody] AuthRequest request, CancellationToken ct)
        {
            var result = await _authservice.LogInAsync(request, ct);

            if (result == null)
            {
                return Unauthorized("Invalid email or password");
            }
            
            return Ok(result);
        }
    }
}
