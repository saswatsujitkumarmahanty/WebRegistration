namespace Domain.Entity
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int Age { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // "User" or "Admin"
    }
}