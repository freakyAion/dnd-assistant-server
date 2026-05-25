using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using static dnd_assistant.Shared.Enums;

namespace dnd_assistant.Models
{
    [Table("Classes")]
    public class Class : GuidEntity
    {
        [Required]
        [MaxLength(255)]
        [RegularExpression(@"^[A-Za-z0-9_\- ]{3,255}$")]
        public required string Name { get; set; }

        [Column(TypeName = "jsonb")]
        public JsonElement? Description { get; set; }

        [Required]
        public DieType HitDie { get; set; }

        [Range(1, 6)]
        public int SkillProficienciesAmount { get; set; }

        [Range(1, 20)]
        public int SubClassUnlockingLevel { get; set; }

        public ArmourProficiency ArmourProficiencies { get; set; } = ArmourProficiency.None;
        public WeaponProficiency WeaponProficiencies { get; set; } = WeaponProficiency.None;
        public AbilityScore SavingThrowProficiencies { get; set; } = AbilityScore.None;

        public AbilityScore? SpellcastingAbility { get; set; }

        public ICollection<ClassFeature> Features { get; set; } = [];
        public ICollection<ClassWeaponProficiency> SpecificWeaponProficiencies { get; set; } = [];

        public ICollection<ClassSkillPool> SkillPool { get; set; } = [];
        public ICollection<ClassSpellSlotProgression> SpellSlotProgression { get; set; } = [];
    }
}
