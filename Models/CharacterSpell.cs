using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_assistant.Models
{
    public class CharacterSpell : GuidEntity
    {
        [Required]
        public Guid CharacterID { get; set; }

        [ForeignKey(nameof(CharacterID))]
        public virtual Character Character { get; set; } = null!;

        [Required]
        public Guid SpellID { get; set; }

        [ForeignKey(nameof(SpellID))]
        public virtual Spell Spell { get; set; } = null!;

        public bool IsPrepared { get; set; } = false;

        public bool IsAlwaysPrepared { get; set; } = false;
    }
}