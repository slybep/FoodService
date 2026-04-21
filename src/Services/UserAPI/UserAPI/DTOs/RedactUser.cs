namespace UserAPI.DTOs
{
    public class RedactUser
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string AvatarUrl { get; set; } = string.Empty;
    }
}
