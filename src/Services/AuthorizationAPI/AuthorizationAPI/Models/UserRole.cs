namespace AuthorizationAPI.Models
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public User User { get; set; } = new User();
        public Role Role { get; set; } = new Role();
    }
}
