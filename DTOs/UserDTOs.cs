using dnd_assistant.Models;
using System.ComponentModel.DataAnnotations;

namespace dnd_assistant.DTOs
{
    public class CreateUserDTO
    {
        [Required]
        [MaxLength(255)]
        [RegularExpression(@"^[A-Za-z0-9_\- ]{3,255}$", ErrorMessage = "Name must be 3–255 characters and contain only letters, numbers, spaces, underscores or hyphens.")]
        public required string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [MaxLength(100)]
        public required string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
        public required string Password { get; set; }

        [Required]
        [EnumDataType(typeof(UserRole), ErrorMessage = "Invalid role.")]
        public UserRole Role { get; set; } = UserRole.Player;
    }

    public class ReturnUserDTO
    {
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public required string Email { get; set; }

        [Required]
        [EnumDataType(typeof(UserRole))]
        public UserRole Role { get; set; } = UserRole.Player;
    }

    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public required string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
        public required string Password { get; set; }
    }
}
