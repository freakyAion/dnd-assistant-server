using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_assistant.Models
{
    public class SessionEvent : GuidEntity
    {
        [Required]
        public Guid SessionID { get; set; }

        [ForeignKey(nameof(SessionID))]
        public Session? Session { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Required]
        public string Content { get; set; } = string.Empty;

        public string Severity { get; set; } = "Info";
    }
}