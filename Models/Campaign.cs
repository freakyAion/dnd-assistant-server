using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_assistant.Models
{
    public class Campaign : GuidEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Guid WorldID { get; set; }

        [ForeignKey(nameof(WorldID))]
        public World? World { get; set; }

        [Required]
        public Guid DungeonMasterID { get; set; }

        [ForeignKey(nameof(DungeonMasterID))]
        public User? DungeonMaster { get; set; }

        public string Notes { get; set; } = string.Empty;
        public string InviteCode { get; set; } = string.Empty;

        public ICollection<Session> Sessions { get; set; } = new List<Session>();
        public ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}