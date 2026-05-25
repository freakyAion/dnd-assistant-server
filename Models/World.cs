using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static dnd_assistant.Shared.Enums;

namespace dnd_assistant.Models
{
    [Table("Worlds")]
    public class World : GuidEntity
    {
        [Required]
        public Guid OwnerID { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(OwnerID))]
        public User Owner { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Pitch { get; set; }

        [Column(TypeName = "jsonb")]
        public string? Description { get; set; }

        public bool IsPublic { get; set; }
    }

    [Table("WorldAccess")]
    public class WorldAccess : GuidEntity
    {
        [Required]
        public Guid WorldID { get; set; }
        public World World { get; set; } = null!;

        [Required]
        public Guid UserID { get; set; }
        [JsonIgnore]
        public User User { get; set; } = null!;

        public AccessRole Role { get; set; }
    }

    [Table("Campaigns")]
    public class Campaign : GuidEntity
    {
        [Required]
        public Guid CreatorID { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(CreatorID))]
        public User Creator { get; set; } = null!;

        [Required]
        public Guid WorldID { get; set; }
        [JsonIgnore]
        public World World { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(100)]
        public string? System { get; set; }

        [MaxLength(500)]
        public string? Pitch { get; set; }

        [Column(TypeName = "jsonb")]
        public string? Description { get; set; }

        public int Level { get; set; }
        public int SessionNumber { get; set; }
        public bool IsPublic { get; set; }
    }

    [Table("CampaignAccess")]
    public class CampaignAccess : GuidEntity
    {
        [Required]
        public Guid CampaignID { get; set; }
        public Campaign Campaign { get; set; } = null!;

        [Required]
        public Guid UserID { get; set; }
        [JsonIgnore]
        public User User { get; set; } = null!;

        public AccessRole Role { get; set; }
    }

    [Table("Locations")]
    public class Location : GuidEntity
    {
        [Required]
        public Guid WorldID { get; set; }
        [JsonIgnore]
        public World World { get; set; } = null!;

        [Required]
        public Guid CreatorID { get; set; }

        public Guid? ParentLocationID { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(ParentLocationID))]
        public Location? ParentLocation { get; set; }

        public double? X { get; set; }
        public double? Y { get; set; }

        [MaxLength(100)]
        public string? Type { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Pitch { get; set; }

        [Column(TypeName = "jsonb")]
        public string? Description { get; set; }
    }

    [Table("Events")]
    public class Event : GuidEntity
    {
        [Required]
        public Guid WorldID { get; set; }
        [JsonIgnore]
        public World World { get; set; } = null!;

        [Required]
        public Guid CreatorID { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Pitch { get; set; }

        [Column(TypeName = "jsonb")]
        public string? Description { get; set; }

        [MaxLength(100)]
        public string? Time { get; set; }

        public bool IsSecret { get; set; }
    }

    [Table("Sessions")]
    public class Session : GuidEntity
    {
        [Required]
        public Guid CreatorID { get; set; }

        [Required]
        public Guid CampaignID { get; set; }
        [JsonIgnore]
        public Campaign Campaign { get; set; } = null!;

        public int Order { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Pitch { get; set; }

        [Column(TypeName = "jsonb")]
        public string? Description { get; set; }

        public bool IsSecret { get; set; }
    }

    [Table("Organisations")]
    public class Organisation : GuidEntity
    {
        [Required]
        public Guid WorldID { get; set; }
        [JsonIgnore]
        public World World { get; set; } = null!;

        [Required]
        public Guid CreatorID { get; set; }

        public Guid? LocationID { get; set; }
        [JsonIgnore]
        public Location? Location { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(100)]
        public string? Type { get; set; }

        [MaxLength(500)]
        public string? Pitch { get; set; }

        [Column(TypeName = "jsonb")]
        public string? Description { get; set; }

        public bool IsSecret { get; set; }
    }

    [Table("NPCs")]
    public class NPC : GuidEntity
    {
        [Required]
        public Guid WorldID { get; set; }
        [JsonIgnore]
        public World World { get; set; } = null!;

        [Required]
        public Guid CreatorID { get; set; }

        public Guid? LocationID { get; set; }
        [JsonIgnore]
        public Location? Location { get; set; }

        public Guid? OrganisationID { get; set; }
        [JsonIgnore]
        public Organisation? Organisation { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [Required]
        public Guid SpeciesID { get; set; }
        [JsonIgnore]
        public Species Species { get; set; } = null!;

        [MaxLength(500)]
        public string? Pitch { get; set; }

        [Column(TypeName = "jsonb")]
        public string? Description { get; set; }

        public bool IsSecret { get; set; }
    }
}