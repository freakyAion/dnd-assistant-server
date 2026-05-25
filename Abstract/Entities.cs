using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_assistant.Abstract
{
    public abstract class Entity<T>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual T ID { get; set; } = default;

        public abstract T GetID();
    }

    public abstract class GuidEntity : Entity<Guid>
    {
        public override Guid ID { get; set; } = Guid.NewGuid();

        public override Guid GetID() => ID;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public void Touch()
        {
            LastUpdatedAt = DateTime.UtcNow;
        }
    }
}
