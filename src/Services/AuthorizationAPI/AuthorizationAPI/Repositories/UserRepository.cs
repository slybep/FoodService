using AuthorizationAPI;
using AuthorizationAPI.Models;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _context;
    public UserRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct)
        => await _context.Users.AnyAsync(u => u.Email == email, ct);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task AddAsync(User user, CancellationToken ct)
        => await _context.Users.AddAsync(user, ct);

    public async Task SaveChangesAsync(CancellationToken ct)
        => await _context.SaveChangesAsync(ct);
    public async Task<User?> FindByEmail(string email, CancellationToken ct)
        => await _context.Users.FirstOrDefaultAsync();

}