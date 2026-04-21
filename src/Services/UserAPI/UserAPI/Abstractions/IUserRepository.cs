using UserAPI.DTOs;
using UserAPI.Models;

namespace UserAPI.Abstractions
{
    public interface IUserRepository
    {
        Task CreateUser(User user, CancellationToken ct);
        Task<bool> DeleteUser(Guid Id, CancellationToken ct);
        Task<RedactUserRequest?> EditUser(Guid Id, RedactUser user, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
        Task<User?> GetUserById(Guid Id, CancellationToken ct);
    }
}