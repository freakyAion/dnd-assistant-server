using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_assistant.Models
{
    [Table("Users")]
    public class User : GuidEntity
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
        [MaxLength(256)]
        public required string PasswordHash { get; set; }

        [Required]
        public UserRole Role { get; set; } = UserRole.Player;
    }

    public enum UserRole
    {
        Player = 0,
        Moderator = 1,
        Admin = 2
    }
}
