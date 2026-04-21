using UserAPI.DTOs;
using UserAPI.Models;

namespace UserAPI.Abstractions
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(User user, CancellationToken ct);
        Task<bool> DeleteUser(Guid id, CancellationToken ct);
        Task<RedactUserRequest?> EditUserAsync(Guid id, RedactUser user, CancellationToken ct);
        Task<User?> GetUserById(Guid id, CancellationToken ct);
    }
}