using AuthorizationAPI.Models;

namespace AuthorizationAPI.Abstractions
{
    public interface IUserRoleRepository
    {
        Task AddAsync(UserRole userRole, CancellationToken ct);
        Task AddRangeAsync(IEnumerable<UserRole> userRoles, CancellationToken ct);
        Task<List<UserRole>> GetByUserIdAsync(Guid userId);
        Task RemoveRangeAsync(IEnumerable<UserRole> userRoles, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
    }
}