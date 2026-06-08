using dnd_assistant.DB;
using dnd_assistant.Models;
using System.Text.Json;
using static dnd_assistant.Shared.Enums;

namespace dnd_assistant.Data
{
    public static class DbSeeder
    {
        public static void Seed(MyDbContext context)
        {
            // Ensure schema is updated before attempting to seed data
            context.Database.EnsureCreated();

            SeedRules(context);
            SeedSpells(context);
            SeedItems(context);
            SeedSpecies(context);
            SeedBackgrounds(context);
            SeedClasses(context);
        }

        private static void SeedRules(MyDbContext context)
        {
            if (context.Rules.Any()) return;

            context.Rules.AddRange(
                new Models.Rule
                {
                    Title = "Actions in Combat",
                    Slug = "combat-actions",
                    Category = Models.Rule.RuleCategory.Combat,
                    Content = JsonDocument.Parse(@"{
                        ""blocks"": [
                            { ""type"": ""heading"", ""text"": ""Actions in Combat"" },
                            { ""type"": ""paragraph"", ""text"": ""When you take your action on your turn, you can take one of the actions presented here: Attack, Cast a Spell, Dash, Disengage, Dodge, Help, Hide, Ready, Search, or Use an Object."" }
                        ]
                    }")
                },
                new Models.Rule
                {
                    Title = "Movement and Position",
                    Slug = "movement",
                    Category = Models.Rule.RuleCategory.Combat,
                    Content = JsonDocument.Parse(@"{
                        ""blocks"": [
                            { ""type"": ""heading"", ""text"": ""Movement and Position"" },
                            { ""type"": ""paragraph"", ""text"": ""On your turn, you can move a distance up to your speed. You can use as much or as little of your speed as you like."" }
                        ]
                    }")
                }
            );
            context.SaveChanges();
        }

        private static void SeedSpells(MyDbContext context)
        {
            if (context.Spells.Any()) return;

            context.Spells.AddRange(
                new Spell
                {
                    Name = "Fireball",
                    Level = 3,
                    School = Spell.MagicSchool.Evocation,
                    CastingTimeValue = 1,
                    CastingTimeType = Spell.CastingTimeUnit.Action,
                    RangeUnits = Spell.RangeUnit.Feet,
                    RangeValue = 150,
                    AoeType = AreaOfEffectShape.Sphere,
                    AoeValue = 20,
                    // V (1) + S (2) + M (4) = 7
                    Components = Spell.ComponentFlags.Verbal | Spell.ComponentFlags.Somatic | Spell.ComponentFlags.Material,
                    MaterialComponents = "A tiny ball of bat guano and sulfur.",
                    MaterialCostGp = 0,
                    MaterialConsumed = false,
                    DurationUnits = Spell.DurationUnit.Instantaneous,
                    RequiresConcentration = false,
                    Description = JsonDocument.Parse(@"{
                        ""blocks"": [
                            { ""type"": ""paragraph"", ""text"": ""A bright streak flashes from your pointing finger to a point you choose within range and then blossoms with a low roar into an explosion of flame."" },
                            { ""type"": ""mechanics"", ""damage"": ""8d6"", ""damageType"": ""Fire"", ""saveType"": ""Dexterity"" }
                        ]
                    }")
                },
                new Spell
                {
                    Name = "Cure Wounds",
                    Level = 1,
                    School = Spell.MagicSchool.Evocation,
                    CastingTimeValue = 1,
                    CastingTimeType = Spell.CastingTimeUnit.Action,
                    RangeUnits = Spell.RangeUnit.Touch,
                    Components = Spell.ComponentFlags.Verbal | Spell.ComponentFlags.Somatic,
                    DurationUnits = Spell.DurationUnit.Instantaneous,
                    RequiresConcentration = false,
                    Description = JsonDocument.Parse(@"{
                        ""blocks"": [
                            { ""type"": ""paragraph"", ""text"": ""A creature you touch regains a number of hit points equal to 1d8 + your spellcasting ability modifier."" }
                        ]
                    }")
                }
            );
            context.SaveChanges();
        }

        private static void SeedItems(MyDbContext context)
        {
            if (context.Items.Any()) return;

            context.Items.AddRange(
                new Item
                {
                    Name = "Longsword",
                    Type = Item.ItemType.Weapon,
                    Rarity = Item.ItemRarity.Mundane,
                    Weight = 3.0m,
                    CostValue = 15,
                    CostCurrency = Currency.Gp,
                    DamageDiceQuantity = 1,
                    DamageDiceSides = 8,
                    DamageType = Item.WeaponDamageType.Slashing,
                    Properties = Item.PropertyFlags.Versatile, // Value: 32
                    Description = JsonDocument.Parse(@"{
                        ""blocks"": [
                            { ""type"": ""paragraph"", ""text"": ""A classic martial melee weapon usable with one or two hands."" }
                        ]
                    }")
                },
                new Item
                {
                    Name = "Plate Armor",
                    Type = Item.ItemType.Armor,
                    Rarity = Item.ItemRarity.Mundane,
                    Weight = 65.0m,
                    CostValue = 1500,
                    CostCurrency = Currency.Gp,
                    AcValue = 18,
                    AcDexBonusType = Item.DexBonusType.None,
                    StrengthRequirement = 15,
                    StealthDisadvantage = true,
                    Description = JsonDocument.Parse(@"{
                        ""blocks"": [
                            { ""type"": ""paragraph"", ""text"": ""Plate consists of shaped, interlocking metal plates to cover the entire body."" }
                        ]
                    }")
                }
            );
            context.SaveChanges();
        }

        private static void SeedSpecies(MyDbContext context)
        {
            if (context.Species.Any()) return;

            var darkvision = new SpeciesTrait
            {
                Name = "Darkvision",
                Description = JsonDocument.Parse(@"{""text"": ""You can see in dim light within 60 feet of you as if it were bright light, and in darkness as if it were dim light.""}")
            };
            context.SpeciesTraits.Add(darkvision);

            context.Species.Add(new Species
            {
                Name = "Elf",
                Size = Species.CreatureSize.Medium,
                BaseSpeed = 30,
                Description = JsonDocument.Parse(@"{""text"": ""Elves are a magical people of sublime grace, living in the world but not entirely part of it.""}"),
                Traits = new List<SpeciesTrait> { darkvision }
            });
            context.SaveChanges();
        }

        private static void SeedBackgrounds(MyDbContext context)
        {
            if (context.Backgrounds.Any()) return;

            var shelter = new BackgroundFeature
            {
                Name = "Shelter of the Faithful",
                Description = JsonDocument.Parse(@"{""text"": ""As an acolyte, you command the respect of those who share your faith, and you can perform the religious ceremonies of your given deity.""}")
            };
            context.BackgroundFeatures.Add(shelter);

            context.Backgrounds.Add(new Background
            {
                Name = "Acolyte",
                Description = JsonDocument.Parse(@"{""text"": ""You have spent your life in the service of a temple to a specific deity or pantheon of gods.""}"),
                Features = new List<BackgroundFeature> { shelter }
            });
            context.SaveChanges();
        }

        private static void SeedClasses(MyDbContext context)
        {
            if (context.Classes.Any()) return;

            // Establish class features registry entries
            var spellcasting = new ClassFeature
            {
                Name = "Spellcasting (Cleric)",
                Description = JsonDocument.Parse(@"{""text"": ""You draw divine power from your deity to cast spells.""}")
            };
            var divineDomain = new ClassFeature
            {
                Name = "Divine Domain",
                Description = JsonDocument.Parse(@"{""text"": ""Choose one domain related to your deity. Your choice grants you domain spells and other features.""}")
            };
            context.ClassFeatures.AddRange(spellcasting, divineDomain);

            var cleric = new Class
            {
                Name = "Cleric",
                HitDieSides = 8,
                // Wisdom (16) + Charisma (32) = 48
                SavingThrows = Class.SavingThrowFlags.Wisdom | Class.SavingThrowFlags.Charisma,
                Description = JsonDocument.Parse(@"{""text"": ""A priestly champion who wields divine magic in service of a higher power.""}")
            };

            // Associate available spell lists to the class tracking context vector
            var cureWounds = context.Spells.FirstOrDefault(s => s.Name == "Cure Wounds");
            if (cureWounds != null) cleric.Spells.Add(cureWounds);

            context.Classes.Add(cleric);
            context.SaveChanges(); // Persist to establish ClassId foreign keys

            // Build out level milestones inside progression structures
            context.ClassLevelProgressions.Add(new ClassLevelProgression
            {
                ClassId = cleric.ID,
                Level = 1,
                ProficiencyBonus = 2,
                SpellSlots = JsonDocument.Parse(@"{""1"": 2}"), // 2 first-level slots
                Features = new List<ClassFeature> { spellcasting, divineDomain }
            });
            context.SaveChanges();
        }
    }
}