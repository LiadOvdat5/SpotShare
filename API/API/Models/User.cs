namespace API.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }

        public int RoleId { get; set; }
    }
}
