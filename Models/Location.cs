using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Nodes;

namespace dnd_assistant.Models
{
    public class Location : GuidEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Guid WorldID { get; set; }

        [ForeignKey(nameof(WorldID))]
        public World? World { get; set; }

        public string Type { get; set; } = string.Empty;

        [Column(TypeName = "jsonb")]
        public JsonNode Description { get; set; } = JsonNode.Parse("{\"blocks\":[]}");

        public Guid? ParentLocationID { get; set; }

        public double? X { get; set; }
        public double? Y { get; set; }
    }
}