using AuthorizationAPI.DTOs;

namespace AuthorizationAPI.Abstractions
{
    public interface IAuthService
    {
        Task<AuthResponse?> LogInAsync(AuthRequest request, CancellationToken ct);
        Task<AuthResponse?> RegisterAsync(RegRequest request, CancellationToken ct);
    }
}