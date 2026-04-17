using AuthorizationAPI.Models;

namespace AuthorizationAPI.Abstractions
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        string? ValidateToken(string token);
    }
}