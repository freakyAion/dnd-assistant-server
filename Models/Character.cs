using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using static dnd_assistant.Shared.Enums;

namespace dnd_assistant.Models
{
    public class Character : GuidEntity
    {
        [Required]
        public Guid UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public virtual User User { get; set; } = null!;

        public bool IsPublic { get; set; } = false;

        #region Bio, Identity & Roleplay Details

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Guid SpeciesID { get; set; }

        [ForeignKey(nameof(SpeciesID))]
        public virtual Species Species { get; set; } = null!;

        [Required]
        public Guid BackgroundID { get; set; }

        [ForeignKey(nameof(BackgroundID))]
        public virtual Background Background { get; set; } = null!;

        [Required]
        public Guid ClassID { get; set; }

        [ForeignKey(nameof(ClassID))]
        public virtual Class Class { get; set; } = null!;

        [Range(1, 20)]
        public int Level { get; set; } = 1;

        public int ExperiencePoints { get; set; } = 0;

        [Required]
        [MaxLength(50)]
        public string Alignment { get; set; } = "Neutral";

        [Required]
        public string Biography { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Age { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Height { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Weight { get; set; } = string.Empty;

        [Required]
        public string PhysicalAppearance { get; set; } = string.Empty;

        #endregion

        #region Core Ability Scores

        public int Strength { get; set; } = 10;
        public int Dexterity { get; set; } = 10;
        public int Constitution { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public int Wisdom { get; set; } = 10;
        public int Charisma { get; set; } = 10;

        #endregion

        #region Combat Vitals & Session States

        public int CurrentHitPoints { get; set; }
        public int MaxHitPoints { get; set; }
        public int TemporaryHitPoints { get; set; } = 0;

        [Range(0, 3)]
        public int DeathSaveSuccesses { get; set; } = 0;

        [Range(0, 3)]
        public int DeathSaveFailures { get; set; } = 0;

        public int CurrentSpeedOverride { get; set; } = 0;

        [Column(TypeName = "jsonb")]
        public JsonDocument ActiveConditions { get; set; }

        public Guid? CampaignID { get; set; }

        [ForeignKey(nameof(CampaignID))]
        public Campaign? Campaign { get; set; }

        #endregion

        #region Proficiencies & Resource Allocations

        public List<Skill> CustomSkillProficiencies { get; set; } = new();

        [Required]
        public int[] ExpendedSpellSlots { get; set; } = new int[9];

        public virtual ICollection<CharacterItem> Inventory { get; set; } = new List<CharacterItem>();

        public virtual ICollection<CharacterSpell> Spells { get; set; } = new List<CharacterSpell>();

        #endregion
    }
}