using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UserUpdateDTO
    {
        [StringLength(100, ErrorMessage = "Full name cannot be longer than 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full name can only contain letters and spaces")]
        public string? FullName { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
        public string? Username { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string? Email { get; set; } = string.Empty;

        public string? Phone { get; set; } = string.Empty;
    }
}
