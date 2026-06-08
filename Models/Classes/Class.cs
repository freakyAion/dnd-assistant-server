using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace dnd_assistant.Models
{
    public class Class : GuidEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public int HitDieSides { get; set; }

        [Required]
        [Column(TypeName = "jsonb")]
        public JsonDocument Description { get; set; }

        public SavingThrowFlags SavingThrows { get; set; }

        public virtual ICollection<ClassLevelProgression> Progressions { get; set; } = new List<ClassLevelProgression>();
        public virtual ICollection<Spell> Spells { get; set; } = new List<Spell>();

        [Flags]
        public enum SavingThrowFlags
        {
            None = 0,
            Strength = 1 << 0,
            Dexterity = 1 << 1,
            Constitution = 1 << 2,
            Intelligence = 1 << 3,
            Wisdom = 1 << 4,
            Charisma = 1 << 5
        }
    }
}