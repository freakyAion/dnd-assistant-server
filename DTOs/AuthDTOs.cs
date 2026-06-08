using System.ComponentModel.DataAnnotations;

namespace dnd_assistant.DTOs
{
    public record RegisterRequest(
        [Required]
        [RegularExpression(@"^[A-Za-z0-9_\- ]{3,255}$", ErrorMessage = "Name must be 3–255 characters and contain only letters, numbers, spaces, underscores or hyphens.")]
        string Name,

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [MaxLength(100)]
        string Email,

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        string Password
    );

    public record LoginRequest(
        [Required]
        [EmailAddress]
        string Email,

        [Required]
        string Password
    );

    public record RefreshTokenRequest(
        [Required]
        string RefreshToken
    );

    public record AuthResponse(
        string AccessToken,
        string RefreshToken,
        string Name,
        string Email,
        string Role
    );
}