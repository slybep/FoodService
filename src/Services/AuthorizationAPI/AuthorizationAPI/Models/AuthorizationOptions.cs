namespace AuthorizationAPI.Models
{
    public class AuthorizationOptions
    {
        public RolePermission[] RolePermissions { get; set; } = [];

        public class RolePermission
        {
            public string Role { get; set; } = string.Empty;
            public string[] Permissions { get; set; } = [];
        }
    }
}
