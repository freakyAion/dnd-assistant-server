using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_assistant.Models
{
    public class Session : GuidEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Guid CampaignID { get; set; }

        [ForeignKey(nameof(CampaignID))]
        public Campaign? Campaign { get; set; }

        public int SessionNumber { get; set; }
        public DateTime ScheduledAt { get; set; } = DateTime.UtcNow;
        public string Summary { get; set; } = string.Empty;

        public ICollection<SessionEvent> Logs { get; set; } = new List<SessionEvent>();
    }
}