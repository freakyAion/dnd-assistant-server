using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Nodes;

namespace dnd_assistant.Models
{
    public class World : GuidEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Guid OwnerID { get; set; }

        [ForeignKey(nameof(OwnerID))]
        public User? Owner { get; set; }

        [Column(TypeName = "jsonb")]
        public JsonNode Description { get; set; } = JsonNode.Parse("{\"blocks\":[]}");

        public bool IsPublic { get; set; } = false;

        public ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();
        public ICollection<Npc> Npcs { get; set; } = new List<Npc>();
        public ICollection<Location> Locations { get; set; } = new List<Location>();
        public ICollection<WorldEvent> HistoricalEvents { get; set; } = new List<WorldEvent>();

        public string MapImageUrl { get; set; } = string.Empty;
    }
}