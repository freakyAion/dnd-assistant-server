using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using static dnd_assistant.Shared.Enums;

namespace dnd_assistant.Models
{
    public class Spell : GuidEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 9)]
        public int Level { get; set; }

        public MagicSchool School { get; set; }

        [Required]
        [Column(TypeName = "jsonb")]
        public required JsonDocument Description { get; set; }

        public string? HigherLevelDescription { get; set; }

        public int CastingTimeValue { get; set; } = 1;
        public CastingTimeUnit CastingTimeType { get; set; }
        [MaxLength(255)]
        public string? CastingTimeCondition { get; set; }

        public RangeUnit RangeUnits { get; set; }
        public int? RangeValue { get; set; }
        public AreaOfEffectShape? AoeType { get; set; }
        public int? AoeValue { get; set; }

        public ComponentFlags Components { get; set; }
        public string? MaterialComponents { get; set; }
        public int MaterialCostGp { get; set; } = 0;
        public bool MaterialConsumed { get; set; } = false;

        public DurationUnit DurationUnits { get; set; }
        public int? DurationValue { get; set; }
        public bool RequiresConcentration { get; set; } = false;

        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

        #region Model-Specific Enums

        public enum MagicSchool
        {
            Abjuration,
            Conjuration,
            Divination,
            Enchantment,
            Evocation,
            Illusion,
            Necromancy,
            Transmutation
        }

        public enum CastingTimeUnit
        {
            Action,
            BonusAction,
            Reaction,
            Minute,
            Hour,
            Special
        }

        public enum RangeUnit
        {
            Self,
            Touch,
            Feet,
            Miles,
            Sight,
            Unlimited,
            Special
        }

        [Flags]
        public enum ComponentFlags
        {
            None = 0,
            Verbal = 1 << 0,   // 1
            Somatic = 1 << 1,  // 2
            Material = 1 << 2  // 4
        }

        public enum DurationUnit
        {
            Instantaneous,
            Round,
            Minute,
            Hour,
            Day,
            Special,
            UntilDispelled
        }

        #endregion
    }
}