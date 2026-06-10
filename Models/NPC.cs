using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Nodes;

namespace dnd_assistant.Models
{
    public class Npc : GuidEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Guid WorldID { get; set; }

        [ForeignKey(nameof(WorldID))]
        public World? World { get; set; }

        public string Race { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
        public string Alignment { get; set; } = "Neutral";

        [Column(TypeName = "jsonb")]
        public JsonNode Description { get; set; } = JsonNode.Parse("{\"blocks\":[]}");

        public bool IsSecret { get; set; } = false;
    }
}