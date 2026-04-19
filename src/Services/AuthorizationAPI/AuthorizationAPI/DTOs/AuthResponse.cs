namespace AuthorizationAPI.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public Guid UserId { get; set; } 
        public string Email { get; set; } = string.Empty;
        public List<string> Role { get; set; } = [];
        
    }
}
