using System.Text.Json;
using static dnd_assistant.Shared.Enums;

namespace dnd_assistant.DTOs
{
    public class FeatureDTO
    {
        public required string Name { get; set; }
        public JsonElement Description { get; set; }
        public int UnlockingLevel { get; set; }
    }

    public class WeaponProficiencyDTO
    {
        public Guid WeaponID { get; set; }
        public required string Weapon { get; set; }
    }

    public class SpellSlotProgressionDTO
    {
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

    public class ReturnClassDTO
    {
        public required string Name { get; set; }
        public required JsonElement Description { get; set; }
        public DieType HitDie { get; set; }
        public int SkillProficienciesAmount { get; set; }
        public int SubClassUnlockingLevel { get; set; }
        public ArmourProficiency ArmourProficiencies { get; set; }
        public WeaponProficiency WeaponProficiencies { get; set; }
        public AbilityScore SavingThrowProficiencies { get; set; }
        public AbilityScore? SpellcastingAbility { get; set; }

        public ICollection<FeatureDTO> Features { get; set; } = [];
        public ICollection<WeaponProficiencyDTO> SpecificWeaponProficiencies { get; set; } = [];
        public ICollection<SpellSlotProgressionDTO> SpellSlotProgression { get; set; } = [];

        public ICollection<Skill> SkillPool { get; set; } = [];
    }
}