using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static dnd_assistant.Shared.Enums;

namespace dnd_assistant.Models
{
    [Table("Classes")]
    public class Class : GuidEntity
    {
        [Required]
        [MaxLength(255)]
        [RegularExpression(@"^[A-Za-z0-9_\- ]{3,255}$", ErrorMessage = "Name must be 3–255 characters and contain only letters, numbers, spaces, underscores or hyphens.")]
        public required string Name { get; set; }

        public string? Description { get; set; } = "TODO: JSON Handling via JSONB column";

        [Required]
        public DieType HitDie { get; set; }

        [Required]
        public int SkillProficienciesAmount { get; set; }

        [Required]
        public int SubClassUnlockingLevel { get; set; }
    }
}
