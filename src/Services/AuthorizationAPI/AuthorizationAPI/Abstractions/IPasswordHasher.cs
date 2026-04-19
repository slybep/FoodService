namespace AuthorizationAPI.Abstractions
{
    public interface IPasswordHasher
    {
        string Generate(string password);
        bool Verify(string password, string HashedPassword);
    }
}