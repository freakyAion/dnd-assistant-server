using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_assistant.Models
{
    [Table("Spells")]
    public class Spell : GuidEntity
    {
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Pitch { get; set; }

        [Column(TypeName = "jsonb")]
        public string? Description { get; set; }

        [Required]
        [Range(0, 9)]
        public int Level { get; set; } // 0 for cantrip

        [MaxLength(100)]
        public string? School { get; set; }

        [MaxLength(100)]
        public string? CastingTime { get; set; }

        [MaxLength(100)]
        public string? Range { get; set; }

        [MaxLength(100)]
        public string? Components { get; set; } // e.g., "V, S, M"
        // TODO: Markers.

        [MaxLength(100)]
        public string? Duration { get; set; }

        [MaxLength(255)]
        public string? Source { get; set; }
    }
}