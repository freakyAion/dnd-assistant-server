using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace dnd_assistant.Models
{
    [Table("Species")]
    public class Species : GuidEntity
    {
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Pitch { get; set; }

        [Column(TypeName = "jsonb")]
        public string? Description { get; set; }

        [Column(TypeName = "jsonb")]
        public string? Bonuses { get; set; }

        [MaxLength(255)]
        public string? Source { get; set; }

        public ICollection<SubSpecies> SubSpecies { get; set; } = [];
    }

    [Table("SubSpecies")]
    public class SubSpecies : GuidEntity
    {
        [Required]
        public Guid SpeciesID { get; set; }

        [ForeignKey(nameof(SpeciesID))]
        [JsonIgnore]
        public Species Species { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Pitch { get; set; }

        [Column(TypeName = "jsonb")]
        public string? Description { get; set; }

        [Column(TypeName = "jsonb")]
        public string? Bonuses { get; set; }

        [MaxLength(255)]
        public string? Source { get; set; }
    }
}