using AuthorizationAPI.Abstractions;

namespace AuthorizationAPI.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Generate(string password) =>
            BCrypt.Net.BCrypt.HashPassword(password);

        public bool Verify(string password, string HashedPassword) =>
            BCrypt.Net.BCrypt.Verify(password, HashedPassword);
    }
}
