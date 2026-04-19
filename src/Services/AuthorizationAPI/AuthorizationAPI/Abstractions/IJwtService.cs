using AuthorizationAPI.Models;

namespace AuthorizationAPI.Abstractions
{
    public interface IJwtService
    {
        Task<string> GenerateToken(User user);
        string? ValidateToken(string token);
    }
}