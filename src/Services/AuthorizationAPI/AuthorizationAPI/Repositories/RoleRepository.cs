using AuthorizationAPI.Abstractions;
using AuthorizationAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AuthorizationAPI.Data.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AuthDbContext _context;

    public RoleRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetByNameAsync(string name, CancellationToken ct)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == name, ct);
    }

    public async Task<List<Role>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Roles.ToListAsync(ct);
    }

    public async Task<Role?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Roles.FirstOrDefaultAsync(e => e.Id == id, ct);
    }
}