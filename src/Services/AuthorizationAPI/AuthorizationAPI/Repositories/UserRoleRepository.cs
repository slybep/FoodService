using AuthorizationAPI.Abstractions;
using AuthorizationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationAPI.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly AuthDbContext _context;

    public UserRoleRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UserRole userRole, CancellationToken ct)
    {
        await _context.UserRoles.AddAsync(userRole, ct);
    }

    public async Task AddRangeAsync(IEnumerable<UserRole> userRoles, CancellationToken ct)
    {
        await _context.UserRoles.AddRangeAsync(userRoles, ct);
    }

    public async Task RemoveRangeAsync(IEnumerable<UserRole> userRoles, CancellationToken ct)
    {
        _context.UserRoles.RemoveRange(userRoles);
        await Task.CompletedTask; 
    }

    public async Task<List<UserRole>> GetByUserIdAsync(Guid userId)
    {
        return await _context.UserRoles
            .Include(ur => ur.Role)
            .Where(ur => ur.UserId == userId)
            .ToListAsync();
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _context.SaveChangesAsync(ct);
    }
}