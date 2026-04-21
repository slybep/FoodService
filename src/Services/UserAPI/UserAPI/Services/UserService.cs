using UserAPI.Abstractions;
using UserAPI.DTOs;
using UserAPI.Models;
using UserAPI.Repository;

namespace UserAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> CreateUserAsync(User user, CancellationToken ct)
        {
            if (user.Id == Guid.Empty)
            {
                return false;
            }
            await _userRepository.CreateUser(user, ct);
            return true;
        }
        public async Task<RedactUserRequest?> EditUserAsync(Guid id, RedactUser user, CancellationToken ct)
        {
            if (id == Guid.Empty)
            {
                return null;
            }
            return await _userRepository.EditUser(id, user, ct);
        }
        public async Task<bool> DeleteUser(Guid id, CancellationToken ct)
        {
            if (id == Guid.Empty)
            {
                return false;
            }
            return await _userRepository.DeleteUser(id, ct);
        }
        public async Task<User?> GetUserById(Guid id, CancellationToken ct)
        {
            if (id == Guid.Empty)
            {
                return null;
            }
            return await _userRepository.GetUserById(id, ct);
        }
    }
}
