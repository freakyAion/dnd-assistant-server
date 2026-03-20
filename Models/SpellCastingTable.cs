using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_assistant.Models
{
    [Table("ClassSpellSlotProgressions")]
    public class ClassSpellSlotProgression : GuidEntity
    {
        [Required]
        public Guid ClassID { get; set; }
        public Class Class { get; set; } = null!;

        [Required]
        [Range(1, 20)]
        public int ClassLevel { get; set; }

        public int Slot1 { get; set; }
        public int Slot2 { get; set; }
        public int Slot3 { get; set; }
        public int Slot4 { get; set; }
        public int Slot5 { get; set; }
        public int Slot6 { get; set; }
        public int Slot7 { get; set; }
        public int Slot8 { get; set; }
        public int Slot9 { get; set; }

        public int? CantripsKnown { get; set; }
        public int? SpellsKnown { get; set; }
        public int? PactSlotLevel { get; set; }
        public int? PactSlotCount { get; set; }
    }
}
