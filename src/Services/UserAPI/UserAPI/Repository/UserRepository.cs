using UserAPI.Abstractions;
using UserAPI.DTOs;
using UserAPI.Models;

namespace UserAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        public UserRepository(UserDbContext context)
        {
            _context = context;
        }
        public async Task CreateUser(User user, CancellationToken ct)
        {
            await _context.Users.AddAsync(user, ct);
            await SaveChangesAsync(ct);
        }
        public async Task SaveChangesAsync(CancellationToken ct)
            => await _context.SaveChangesAsync(ct);
        public async Task<RedactUserRequest?> EditUser(Guid Id, RedactUser user, CancellationToken ct)
        {
            var result = await _context.Users.FindAsync(Id, ct);
            if (result == null)
            {
                return null;
            }
            result.FirstName = user.Name;
            result.LastName = user.LastName;
            result.Phone = user.Phone;
            result.BirthDate = user.BirthDate;
            await SaveChangesAsync(ct);
            return new RedactUserRequest
            {
                Name = user.Name,
                Phone = user.Phone,
                LastName = user.LastName
            };
        }
        public async Task<bool> DeleteUser(Guid Id, CancellationToken ct)
        {
            var result = await _context.Users.FindAsync(Id, ct);
            if (result == null)
            {
                return false;
            }
            _context.Users.Remove(result);
            await SaveChangesAsync(ct);
            return true;
        }
        public async Task<User?> GetUserById(Guid Id, CancellationToken ct)
        {
            var result = await _context.Users.FindAsync(Id, ct);
            if (result == null)
            {
                return null;
            }
            _context.Users.Remove(result);
            await SaveChangesAsync(ct);
            return new User
            {
                Id = Id,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Phone = result.Phone,
                BirthDate = result.BirthDate,
                CreatedAt = result.CreatedAt,
            };
        }
    }
}
