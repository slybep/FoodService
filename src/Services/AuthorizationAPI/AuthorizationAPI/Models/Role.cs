namespace AuthorizationAPI.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;    
        public ICollection<UserRole> UserRole { get; set; } = new HashSet<UserRole>();
    }
}
