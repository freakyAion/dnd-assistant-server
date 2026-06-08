using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_assistant.Models
{
    public class CharacterItem : GuidEntity
    {
        [Required]
        public Guid CharacterID { get; set; }

        [ForeignKey(nameof(CharacterID))]
        public virtual Character Character { get; set; } = null!;

        [Required]
        public Guid ItemID { get; set; }

        [ForeignKey(nameof(ItemID))]
        public virtual Item Item { get; set; } = null!;

        [Range(1, 9999)]
        public int Quantity { get; set; } = 1;

        public bool IsEquipped { get; set; } = false;

        public bool IsAttuned { get; set; } = false;
    }
}