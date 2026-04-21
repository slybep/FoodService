using Microsoft.AspNetCore.Mvc;
using UserAPI.Abstractions;
using UserAPI.DTOs;
using UserAPI.Models;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("api/internal/profiles")]
    public class UserCreateController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly IConfiguration _configuration;

        public UserCreateController(IUserRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] CreateUserRequest request, CancellationToken ct)
        {
            var expectedApiKey = _configuration["InternalApi:ApiKey"];
            var providedApiKey = Request.Headers["Api-Key"].FirstOrDefault();

            if (string.IsNullOrEmpty(providedApiKey) || providedApiKey != expectedApiKey)
            {
                return Unauthorized("Invalid or missing API key");
            }

            var profile = new User
            {
                Id = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.CreateUser(profile, ct);
            return Ok();
        }
    }
}