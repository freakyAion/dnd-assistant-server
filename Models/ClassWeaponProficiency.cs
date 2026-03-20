using dnd_assistant.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_assistant.Models
{
    [Table("ClassWeaponProficiencies")]
    public class ClassWeaponProficiency : GuidEntity
    {
        [Required]
        public Guid ClassID { get; set; }
        public Class Class { get; set; } = null!;

        [Required]
        public Guid WeaponID { get; set; }
        public string Weapon { get; set; } = "TODO: Replace with Weapons Table Class";
    }

}
