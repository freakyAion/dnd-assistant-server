using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace dnd_assistant.Models
{
    public class Species : GuidEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public CreatureSize Size { get; set; } = CreatureSize.Medium;

        public int BaseSpeed { get; set; } = 30;

        [Required]
        [Column(TypeName = "jsonb")]
        public JsonDocument Description { get; set; }

        public virtual ICollection<SpeciesTrait> Traits { get; set; } = new List<SpeciesTrait>();

        public enum CreatureSize
        {
            Tiny,
            Small,
            Medium,
            Large
        }
    }
}