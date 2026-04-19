using AuthorizationAPI.Abstractions;
using AuthorizationAPI.DTOs;
using AuthorizationAPI.Models;
using AuthorizationAPI.Models.Enums;
using FluentValidation;


namespace AuthorizationAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwt;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<AuthRequest> _loginValidator;
        private readonly IValidator<RegRequest> _regvalidator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRoleRepository _userRole;
        public AuthService(IUserRepository repostiory, 
            IJwtService jwt, 
            IValidator<AuthRequest> loginValidator, 
            IValidator<RegRequest> regValidator,
            IPasswordHasher passwordHasher,
            IUserRoleRepository userRole)
        {
            _loginValidator = loginValidator;
            _regvalidator = regValidator;
            _userRepository = repostiory;
            _jwt = jwt;
            _passwordHasher = passwordHasher;
            _userRole = userRole;
        }
        public async Task<AuthResponse?> RegisterAsync(RegRequest request, CancellationToken ct)
        {
            await _regvalidator.ValidateAndThrowAsync(request);
            var normalizedEmail = request.Email.Trim().ToLowerInvariant();
            if (await _userRepository.EmailExistsAsync(normalizedEmail, ct))
            {
                return null;
            }
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = normalizedEmail,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = _passwordHasher.Generate(request.Password)
            };
            await _userRepository.AddAsync(user, ct);
            await _userRepository.SaveChangesAsync(ct);
            var userRole = (int)Roles.User;

            var userAndRole = new UserRole
            {
                UserId = user.Id,
                RoleId = userRole
            };

            await _userRole.AddAsync(userAndRole, ct);
            await _userRole.SaveChangesAsync(ct);
            
            var token = await _jwt.GenerateToken(user);
            return new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
            };
        }
        public async Task<AuthResponse?> LogInAsync(AuthRequest request, CancellationToken ct)
        {
            await _loginValidator.ValidateAndThrowAsync(request);

            var normalizedEmail = request.Email.Trim().ToLowerInvariant();
            var user = await _userRepository.GetByEmailAsync(normalizedEmail, ct);
            if (user == null)
            {
                return null;
            }
            if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                return null;
            }
            var token = await _jwt.GenerateToken(user);
            var userRoles = await _userRole.GetByUserIdAsync(user.Id);
            var roleNames = userRoles.Select(ur => ur.Role.Name).ToList();
            return new AuthResponse
            {
                UserId = user.Id,
                Token = token,
                Email = user.Email,
                Role = roleNames
            };
        }

    }
}
