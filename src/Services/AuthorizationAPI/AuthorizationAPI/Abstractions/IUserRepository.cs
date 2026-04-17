using AuthorizationAPI.Models;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken ct);
    Task<bool> EmailExistsAsync(string email, CancellationToken ct);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}