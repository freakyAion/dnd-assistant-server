using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_assistant.Models
{
    [Table("ClassFeatures")]
    public class ClassFeature : GuidEntity
    {
        [Required]
        [MaxLength(255)]
        [RegularExpression(@"^[A-Za-z0-9_\- ]{3,255}$", ErrorMessage = "Name must be 3–255 characters and contain only letters, numbers, spaces, underscores or hyphens.")]
        public required string Name { get; set; }

        public string? Description { get; set; } = "TODO: JSON Handling via JSONB column";

        [Required]
        [Range(0, 20, ErrorMessage = "Value out of range.")]
        public int UnlockingLevel { get; set; } = 1;

        [Required]
        public Guid ClassID { get; set; }

        public Class Class { get; set; } = null!;
    }
}
