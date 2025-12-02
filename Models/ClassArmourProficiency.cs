using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_assistant.Models
{
    [Table("ClassArmourProficiencies")]
    public class ClassArmourProficiency : GuidEntity
    {
        [Required]
        public bool ShieldProficiency { get; set; }

        [Required]
        public bool LightProficiency { get; set; }

        [Required]
        public bool MediumProficiency { get; set; }

        [Required]
        public bool HeavyProficiency { get; set; }
    }
}
