using dnd_assistant.Abstract;
using dnd_assistant.Models;
using Microsoft.EntityFrameworkCore;
namespace dnd_assistant.DB
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassFeature> ClassFeatures { get; set; }
        public DbSet<ClassWeaponProficiency> ClassWeaponProficiencies { get; set; }
        public DbSet<ClassSpellSlotProgression> ClassSpellSlotProgressions { get; set; }
        public DbSet<ClassSkillPool> ClassSkillPools { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<SubSpecies> SubSpecies { get; set; }
        public DbSet<Spell> Spells { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Background> Backgrounds { get; set; }
        public DbSet<World> Worlds { get; set; }
        public DbSet<WorldAccess> WorldAccesses { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignAccess> CampaignAccesses { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<NPC> NPCs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Class>()
                .Property(e => e.Description)
                .HasColumnType("jsonb");

            modelBuilder.Entity<ClassFeature>()
                .Property(e => e.Description)
                .HasColumnType("jsonb");

            modelBuilder.Entity<Species>().Property(e => e.Description).HasColumnType("jsonb");
            modelBuilder.Entity<Species>().Property(e => e.Bonuses).HasColumnType("jsonb");

            modelBuilder.Entity<SubSpecies>().Property(e => e.Description).HasColumnType("jsonb");
            modelBuilder.Entity<SubSpecies>().Property(e => e.Bonuses).HasColumnType("jsonb");

            modelBuilder.Entity<Spell>().Property(e => e.Description).HasColumnType("jsonb");

            modelBuilder.Entity<Item>().Property(e => e.Description).HasColumnType("jsonb");
            modelBuilder.Entity<Item>().Property(e => e.Properties).HasColumnType("jsonb");

            modelBuilder.Entity<Background>().Property(e => e.Description).HasColumnType("jsonb");
            modelBuilder.Entity<Background>().Property(e => e.Proficiencies).HasColumnType("jsonb");
            modelBuilder.Entity<Background>().Property(e => e.Equipment).HasColumnType("jsonb");

            modelBuilder.Entity<World>().Property(e => e.Description).HasColumnType("jsonb");
            modelBuilder.Entity<Campaign>().Property(e => e.Description).HasColumnType("jsonb");
            modelBuilder.Entity<Location>().Property(e => e.Description).HasColumnType("jsonb");
            modelBuilder.Entity<Event>().Property(e => e.Description).HasColumnType("jsonb");
            modelBuilder.Entity<Session>().Property(e => e.Description).HasColumnType("jsonb");
            modelBuilder.Entity<Organisation>().Property(e => e.Description).HasColumnType("jsonb");
            modelBuilder.Entity<NPC>().Property(e => e.Description).HasColumnType("jsonb");

            modelBuilder.Entity<Session>()
                .HasIndex(s => new { s.CampaignID, s.Order })
                .IsUnique();

            modelBuilder.Entity<WorldAccess>()
                .HasIndex(w => new { w.WorldID, w.UserID })
                .IsUnique();

            modelBuilder.Entity<CampaignAccess>()
                .HasIndex(c => new { c.CampaignID, c.UserID })
                .IsUnique();
        }

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