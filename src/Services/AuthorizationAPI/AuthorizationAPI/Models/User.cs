using AuthorizationAPI.Abstractions;

namespace AuthorizationAPI.Models
{
    public class User : IDocument
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Email {get; set;} = string.Empty; 
        public string PasswordHash {get; set;} = string.Empty;
        public ICollection<Role> Roles { get; set; } = [];
    }
}
