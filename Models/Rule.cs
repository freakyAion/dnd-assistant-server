using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace dnd_assistant.Models
{
    public class Rule : GuidEntity
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Slug { get; set; } = string.Empty;

        public RuleCategory Category { get; set; }

        [Required]
        [Column(TypeName = "jsonb")]
        public JsonDocument Content { get; set; }

        public enum RuleCategory
        {
            CoreMechanics,
            Combat,
            Adventuring,
            Spellcasting
        }
    }
}