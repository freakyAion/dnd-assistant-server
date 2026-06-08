using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace dnd_assistant.Models
{
    public class BackgroundFeature : GuidEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "jsonb")]
        public JsonDocument Description { get; set; }
    }
}