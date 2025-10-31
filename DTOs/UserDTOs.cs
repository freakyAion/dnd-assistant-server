using dnd_assistant.Models;

namespace dnd_assistant.DTOs
{
    public class CreateUserDTO
    {
        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public UserRole Role { get; set; } = UserRole.Player;
    }

    public class ReturnUserDTO
    {
        public required string Name { get; set; }

        public required string Email { get; set; }

        public UserRole Role { get; set; } = UserRole.Player;
    }
}
