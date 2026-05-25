using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_assistant.Models
{
    [Table("Items")]
    public class Item : GuidEntity
    {
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Pitch { get; set; }

        [Column(TypeName = "jsonb")]
        public string? Description { get; set; }

        [MaxLength(100)]
        public string? Type { get; set; } // e.g., Weapon, Armor, Consumable

        [MaxLength(100)]
        public string? Rarity { get; set; }

        [MaxLength(100)]
        public string? Cost { get; set; }

        [Range(0d, 9999d)]
        public double? Weight { get; set; }

        [Column(TypeName = "jsonb")]
        public string? Properties { get; set; } // JSON for damage dice, AC bonuses, etc.

        [MaxLength(255)]
        public string? Source { get; set; }
    }
}