namespace AuthorizationAPI.Models
{
    public class Permissions
    {
       public int Id {  get; set; }
       public string Name { get; set; } = string.Empty;
        public ICollection<Role> Roles { get; set; } = [];
    }
}
