using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static dnd_assistant.Shared.Enums;

namespace dnd_assistant.Models
{
    [Table("ClassSkillPool")]
    public class ClassSkillPool : GuidEntity
    {
        [Required]
        public Guid ClassID { get; set; }
        public Class Class { get; set; } = null!;

        [Required]
        public Skill Skill { get; set; }
    }
}
