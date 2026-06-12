using dnd_assistant.Abstract;
using dnd_assistant.Models;
using Microsoft.EntityFrameworkCore;

namespace dnd_assistant.DB
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Spell> Spells { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassLevelProgression> ClassLevelProgressions { get; set; }
        public DbSet<ClassFeature> ClassFeatures { get; set; }
        public DbSet<Species> Species => Set<Species>();
        public DbSet<SpeciesTrait> SpeciesTraits { get; set; }
        public DbSet<Background> Backgrounds => Set<Background>();
        public DbSet<BackgroundFeature> BackgroundFeatures { get; set; }
        public DbSet<Models.Rule> Rules { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<CharacterItem> CharacterItems { get; set; }
        public DbSet<CharacterSpell> CharacterSpells { get; set; }
        public DbSet<World> Worlds { get; set; }
        public DbSet<Npc> Npcs { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<WorldEvent> WorldEvents { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionEvent> SessionEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Spell>(entity =>
            {
                entity.HasIndex(s => s.Name).IsUnique();
                entity.Property(s => s.School).HasConversion<string>();
                entity.Property(s => s.CastingTimeType).HasConversion<string>();
                entity.Property(s => s.RangeUnits).HasConversion<string>();
                entity.Property(s => s.DurationUnits).HasConversion<string>();
                entity.Property(s => s.AoeType).HasConversion<string>();
                entity.Property(s => s.Components).HasConversion<int>();
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasIndex(i => i.Name).IsUnique();
                entity.Property(i => i.Type).HasConversion<string>();
                entity.Property(i => i.Rarity).HasConversion<string>();
                entity.Property(i => i.CostCurrency).HasConversion<string>();
                entity.Property(i => i.AcDexBonusType).HasConversion<string>();
                entity.Property(i => i.DamageType).HasConversion<string>();
                entity.Property(i => i.Properties).HasConversion<int>();
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasIndex(c => c.Name).IsUnique();
                entity.Property(c => c.SavingThrows).HasConversion<int>();

                entity.HasMany(c => c.Spells)
                      .WithMany(s => s.Classes)
                      .UsingEntity(j => j.ToTable("ClassSpells"));
            });

            modelBuilder.Entity<ClassFeature>(entity =>
            {
                entity.HasIndex(cf => cf.Name).IsUnique();
            });

            modelBuilder.Entity<ClassLevelProgression>(entity =>
            {
                entity.HasIndex(clp => new { clp.ClassId, clp.Level }).IsUnique();

                entity.HasMany(clp => clp.Features)
                      .WithMany(cf => cf.LevelProgressions)
                      .UsingEntity(j => j.ToTable("ClassLevelProgressionFeatures"));
            });

            modelBuilder.Entity<Species>(entity =>
            {
                entity.HasIndex(s => s.Name).IsUnique();
                entity.Property(s => s.Size).HasConversion<string>();

                entity.HasMany(s => s.Traits)
                      .WithMany()
                      .UsingEntity(j => j.ToTable("SpeciesToTraits"));
            });

            modelBuilder.Entity<SpeciesTrait>(entity =>
            {
                entity.HasIndex(st => st.Name).IsUnique();
                entity.ToTable("SpeciesTraits");
            });

            modelBuilder.Entity<Background>(entity =>
            {
                entity.HasIndex(b => b.Name).IsUnique();

                entity.HasMany(b => b.Features)
                      .WithMany()
                      .UsingEntity(j => j.ToTable("BackgroundToFeatures"));
            });

            modelBuilder.Entity<BackgroundFeature>(entity =>
            {
                entity.HasIndex(bf => bf.Name).IsUnique();
                entity.ToTable("BackgroundFeatures");
            });

            modelBuilder.Entity<Models.Rule>(entity =>
            {
                entity.HasIndex(r => r.Slug).IsUnique();
                entity.Property(r => r.Category).HasConversion<string>();
            });

            modelBuilder.Entity<Character>(entity =>
            {
                entity.HasIndex(c => c.UserID);
                entity.PrimitiveCollection(c => c.CustomSkillProficiencies);
                entity.PrimitiveCollection(c => c.ExpendedSpellSlots);
            });

            modelBuilder.Entity<CharacterItem>(entity =>
            {
                entity.HasIndex(ci => ci.CharacterID);

                entity.HasOne(ci => ci.Character)
                      .WithMany(c => c.Inventory)
                      .HasForeignKey(ci => ci.CharacterID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CharacterSpell>(entity =>
            {
                entity.HasIndex(cs => cs.CharacterID);

                entity.HasOne(cs => cs.Character)
                      .WithMany(c => c.Spells)
                      .HasForeignKey(cs => cs.CharacterID)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.HasMany(c => c.Characters)
                      .WithOne(ch => ch.Campaign)
                      .HasForeignKey(ch => ch.CampaignID)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<World>(entity =>
            {
                entity.HasMany(w => w.Campaigns)
                      .WithOne(c => c.World)
                      .HasForeignKey(c => c.WorldID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(w => w.Npcs)
                      .WithOne(n => n.World)
                      .HasForeignKey(n => n.WorldID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(w => w.Locations)
                      .WithOne(l => l.World)
                      .HasForeignKey(l => l.WorldID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(w => w.HistoricalEvents)
                      .WithOne(e => e.World)
                      .HasForeignKey(e => e.WorldID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasMany(s => s.Logs)
                      .WithOne(se => se.Session)
                      .HasForeignKey(se => se.SessionID)
                      .OnDelete(DeleteBehavior.Cascade);
            });
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