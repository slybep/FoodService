namespace AuthorizationAPI.Abstractions
{
    public interface IUserProfileServiceClient
    {
        Task<bool> CreateProfileAsync(Guid userId, CancellationToken ct = default);
    }
}