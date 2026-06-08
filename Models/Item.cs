using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using static dnd_assistant.Shared.Enums;

namespace dnd_assistant.Models
{
    public class Item : GuidEntity
    {

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public ItemType Type { get; set; }
        public ItemRarity Rarity { get; set; }

        [Required]
        [Column(TypeName = "jsonb")]
        public required JsonDocument Description { get; set; }

        public decimal Weight { get; set; } = 0.0m;
        public int CostValue { get; set; } = 0;
        public Currency CostCurrency { get; set; }

        public bool RequiresAttunement { get; set; } = false;
        public string? AttunementPrerequisites { get; set; }
        public int? StrengthRequirement { get; set; }
        public bool StealthDisadvantage { get; set; } = false;

        public int? AcValue { get; set; }
        public DexBonusType? AcDexBonusType { get; set; }
        public int? DamageDiceQuantity { get; set; }
        public int? DamageDiceSides { get; set; }
        public WeaponDamageType? DamageType { get; set; }
        public PropertyFlags Properties { get; set; }

        public decimal? ContainerCapacityWeight { get; set; }
        public bool IsConsumable { get; set; } = false;
        public bool HasCharges { get; set; } = false;
        public int? MaxCharges { get; set; }
        public string? ChargeResetCondition { get; set; }

        #region Model-Specific Enums

        public enum ItemType
        {
            Armor,
            Weapon,
            AdventuringGear,
            Tool,
            Consumable,
            Container,
            WondrousItem
        }

        public enum ItemRarity
        {
            Mundane,
            Common,
            Uncommon,
            Rare,
            VeryRare,
            Legendary,
            Artifact
        }

        public enum DexBonusType
        {
            None,
            Max2,
            Full
        }

        public enum WeaponDamageType
        {
            Bludgeoning,
            Piercing,
            Slashing,
            Acid,
            Cold,
            Fire,
            Force,
            Lightning,
            Necrotic,
            Poison,
            Psychic,
            Radiant,
            Thunder
        }

        [Flags]
        public enum PropertyFlags
        {
            None = 0,
            Finesse = 1 << 0,
            Heavy = 1 << 1,
            Light = 1 << 2,
            Reach = 1 << 3,
            TwoHanded = 1 << 4,
            Versatile = 1 << 5,
            Thrown = 1 << 6,
            Ammunition = 1 << 7,
            Loading = 1 << 8
        }

        #endregion
    }
}