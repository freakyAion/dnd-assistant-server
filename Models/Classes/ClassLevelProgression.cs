using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace dnd_assistant.Models
{
    public class ClassLevelProgression : GuidEntity
    {
        public Guid ClassId { get; set; }

        [ForeignKey(nameof(ClassId))]
        public virtual Class Class { get; set; } = null!;

        [Range(1, 20)]
        public int Level { get; set; }

        public int ProficiencyBonus { get; set; }

        [Column(TypeName = "jsonb")]
        public JsonDocument? SpellSlots { get; set; }

        public virtual ICollection<ClassFeature> Features { get; set; } = new List<ClassFeature>();
    }
}