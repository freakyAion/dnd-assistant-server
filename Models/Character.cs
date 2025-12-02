using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static dnd_assistant.Shared.Enums;

namespace dnd_assistant.Models
{
    [Table("Characters")]
    public class Character : GuidEntity
    {
        [Required]
        public Guid UserID { get; set; }

        public Class User { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        [RegularExpression(@"^[A-Za-z0-9_\- ]{3,255}$", ErrorMessage = "Name must be 3–255 characters and contain only letters, numbers, spaces, underscores or hyphens.")]
        public required string Name { get; set; }

        [Required]
        [Range(1, 20)]
        public required int Level { get; set; } = 1;

        [Required]
        public Guid StartingClassID { get; set; }

        public Class StartingClass { get; set; } = null!;

        public string? Ideals { get; set; } = "TODO: JSON Handling via JSONB column";
        public string? Bonds { get; set; } = "TODO: JSON Handling via JSONB column";
        public string? Flaws { get; set; } = "TODO: JSON Handling via JSONB column";

        [MaxLength(31)]
        [RegularExpression(@"^[A-Za-z0-9_\- ]{3,255}$", ErrorMessage = "Name must be 3–255 characters and contain only letters, numbers, spaces, underscores or hyphens.")]
        public string? Sex { get; set; } = string.Empty;

        [MaxLength(31)]
        [RegularExpression(@"^[A-Za-z0-9_\- ]{3,255}$", ErrorMessage = "Name must be 3–255 characters and contain only letters, numbers, spaces, underscores or hyphens.")]
        public string? Complexion { get; set; } = string.Empty; // Цвет кожи

        [MaxLength(31)]
        [RegularExpression(@"^[A-Za-z0-9_\- ]{3,255}$", ErrorMessage = "Name must be 3–255 characters and contain only letters, numbers, spaces, underscores or hyphens.")]
        public string? HairColour { get; set; } = string.Empty;

        [MaxLength(31)]
        [RegularExpression(@"^[A-Za-z0-9_\- ]{3,255}$", ErrorMessage = "Name must be 3–255 characters and contain only letters, numbers, spaces, underscores or hyphens.")]
        public string? EyeColour { get; set; } = string.Empty;

        public Alignment? alignment { get; set; } = null!;

        [MaxLength(31)]
        [RegularExpression(@"^[A-Za-z0-9_\- ]{3,255}$", ErrorMessage = "Name must be 3–255 characters and contain only letters, numbers, spaces, underscores or hyphens.")]
        public string? Religion { get; set; } = string.Empty;

        [Range(0d, 999d)]
        public double? Height { get; set; } = null!;

        [Range(0d, 999d)]
        public double? Weight { get; set; } = null!;

        [MaxLength(31)]
        [RegularExpression(@"^[A-Za-z0-9_\- ]{3,255}$", ErrorMessage = "Name must be 3–255 characters and contain only letters, numbers, spaces, underscores or hyphens.")]
        public string? Age { get; set; } = null!;

        public Guid RaceID = Guid.Empty;
        public Guid BackgroundID = Guid.Empty;

        public string? Looks { get; set; } = "TODO: JSON Handling via JSONB column";
        public string? Temper { get; set; } = "TODO: JSON Handling via JSONB column";
        public string? Backstory { get; set; } = "TODO: JSON Handling via JSONB column";
        public string? Statuseffects { get; set; } = "TODO: JSON Handling via JSONB column";

    }
}
