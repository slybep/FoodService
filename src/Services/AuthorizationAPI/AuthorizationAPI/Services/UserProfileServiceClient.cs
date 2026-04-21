using AuthorizationAPI.Abstractions;
using System.Text;
using System.Text.Json;

namespace AuthorizationAPI.Services
{
    public class UserProfileServiceClient : IUserProfileServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserProfileServiceClient> _logger;

        public UserProfileServiceClient(HttpClient httpClient, ILogger<UserProfileServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> CreateProfileAsync(Guid userId, CancellationToken ct = default)
        {
            var request = new { UserId = userId};
            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("/api/internal/profiles", content, ct);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(ct);
                _logger.LogError("Failed to create profile for user {UserId}: {StatusCode} - {Error}",
                    userId, response.StatusCode, error);
                return false;
            }
            return true;
        }
    }
}
