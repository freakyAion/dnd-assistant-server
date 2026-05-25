using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_assistant.Models
{
    [Table("Backgrounds")]
    public class Background : GuidEntity
    {
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Pitch { get; set; }

        [Column(TypeName = "jsonb")]
        public string? Description { get; set; }

        [Column(TypeName = "jsonb")]
        public string? Proficiencies { get; set; }

        [Column(TypeName = "jsonb")]
        public string? Equipment { get; set; }

        [MaxLength(255)]
        public string? Source { get; set; }
    }
}