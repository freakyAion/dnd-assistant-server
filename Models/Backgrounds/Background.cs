using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using static dnd_assistant.Shared.Enums;

namespace dnd_assistant.Models
{
    public class Background : GuidEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "jsonb")]
        public JsonDocument Description { get; set; }

        public List<Skill> SkillProficiencies { get; set; } = new();

        public virtual ICollection<BackgroundFeature> Features { get; set; } = new List<BackgroundFeature>();
    }
}