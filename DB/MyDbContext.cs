using dnd_assistant.Abstract;
using dnd_assistant.Models;
using Microsoft.EntityFrameworkCore;

namespace dnd_assistant.DB
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public override int SaveChanges()
        {
            ApplyAuditRules();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditRules();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditRules()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<GuidEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        var createdProp = entry.Property(nameof(GuidEntity.CreatedAt));
                        if (createdProp.CurrentValue is DateTime created && created == default)
                            createdProp.CurrentValue = now;

                        entry.Entity.Touch();

                        if (entry.Property(nameof(GuidEntity.ID)).CurrentValue is Guid id && id == Guid.Empty)
                            entry.Property(nameof(GuidEntity.ID)).CurrentValue = Guid.NewGuid();
                        break;

                    case EntityState.Modified:
                        entry.Entity.Touch();

                        entry.Property(nameof(GuidEntity.CreatedAt)).IsModified = false;
                        break;

                    default:
                        break;
                }
            }
        }

    }
}
