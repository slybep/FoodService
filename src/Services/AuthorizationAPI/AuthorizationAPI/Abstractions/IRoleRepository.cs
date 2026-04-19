using AuthorizationAPI.Models;

namespace AuthorizationAPI.Abstractions
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllAsync(CancellationToken ct);
        Task<Role?> GetByIdAsync(int id, CancellationToken ct);
        Task<Role?> GetByNameAsync(string name, CancellationToken ct);
    }
}