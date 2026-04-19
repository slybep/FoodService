namespace AuthorizationAPI.Models
{
    public class RolePermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public Role Role { get; set; } 
        public Permissions Permission { get; set; }
    }
}
