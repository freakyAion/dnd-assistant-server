using dnd_assistant.DB;
using dnd_assistant.Models;
using static dnd_assistant.Shared.Enums;

namespace dnd_assistant.Data.Seed
{
    public static class ClassSeedData
    {
        public static void Seed(MyDbContext context)
        {
            if (context.Classes.Any()) return;

            // ─── GUIDs ───────────────────────────────────────────────────────────────
            var barbarianId = Guid.Parse("11111111-0000-0000-0000-000000000001");
            var bardId = Guid.Parse("11111111-0000-0000-0000-000000000002");
            var clericId = Guid.Parse("11111111-0000-0000-0000-000000000003");
            var druidId = Guid.Parse("11111111-0000-0000-0000-000000000004");
            var fighterId = Guid.Parse("11111111-0000-0000-0000-000000000005");
            var monkId = Guid.Parse("11111111-0000-0000-0000-000000000006");
            var paladinId = Guid.Parse("11111111-0000-0000-0000-000000000007");
            var rangerId = Guid.Parse("11111111-0000-0000-0000-000000000008");
            var rogueId = Guid.Parse("11111111-0000-0000-0000-000000000009");
            var sorcererId = Guid.Parse("11111111-0000-0000-0000-000000000010");
            var warlockId = Guid.Parse("11111111-0000-0000-0000-000000000011");
            var wizardId = Guid.Parse("11111111-0000-0000-0000-000000000012");

            // =========================================================================
            // CLASSES
            // =========================================================================
            var classes = new List<Class>
            {
                // ── BARBARIAN ────────────────────────────────────────────────────────
                new Class
                {
                    ID = barbarianId,
                    Name = "Barbarian",
                    Description = """{"summary":"A fierce warrior who can enter a battle rage, drawing on primal forces to shrug off damage and deal devastating blows."}""",
                    HitDie = DieType.D12,
                    SkillProficienciesAmount = 2,
                    SubClassUnlockingLevel = 3,
                    ArmourProficiencies = ArmourProficiency.Light | ArmourProficiency.Medium | ArmourProficiency.Shield,
                    WeaponProficiencies = WeaponProficiency.Simple | WeaponProficiency.Martial,
                    SavingThrowProficiencies = AbilityScore.Strength | AbilityScore.Constitution,
                    SpellcastingAbility = null,
                },

                // ── BARD ─────────────────────────────────────────────────────────────
                new Class
                {
                    ID = bardId,
                    Name = "Bard",
                    Description = """{"summary":"An inspiring magician whose power echoes the music of creation, weaving spells through performance and wit."}""",
                    HitDie = DieType.D8,
                    SkillProficienciesAmount = 3,
                    SubClassUnlockingLevel = 3,
                    ArmourProficiencies = ArmourProficiency.Light,
                    WeaponProficiencies = WeaponProficiency.Simple,
                    SavingThrowProficiencies = AbilityScore.Dexterity | AbilityScore.Charisma,
                    SpellcastingAbility = AbilityScore.Charisma,
                },

                // ── CLERIC ───────────────────────────────────────────────────────────
                new Class
                {
                    ID = clericId,
                    Name = "Cleric",
                    Description = """{"summary":"A priestly champion who wields divine magic in service of a higher power, healing allies and smiting foes."}""",
                    HitDie = DieType.D8,
                    SkillProficienciesAmount = 2,
                    SubClassUnlockingLevel = 1,
                    ArmourProficiencies = ArmourProficiency.Light | ArmourProficiency.Medium | ArmourProficiency.Shield,
                    WeaponProficiencies = WeaponProficiency.Simple,
                    SavingThrowProficiencies = AbilityScore.Wisdom | AbilityScore.Charisma,
                    SpellcastingAbility = AbilityScore.Wisdom,
                },

                // ── DRUID ────────────────────────────────────────────────────────────
                new Class
                {
                    ID = druidId,
                    Name = "Druid",
                    Description = """{"summary":"A priest of the Old Faith wielding the powers of nature and adopting animal forms to serve the natural world."}""",
                    HitDie = DieType.D8,
                    SkillProficienciesAmount = 2,
                    SubClassUnlockingLevel = 2,
                    ArmourProficiencies = ArmourProficiency.Light | ArmourProficiency.Medium | ArmourProficiency.Shield,
                    WeaponProficiencies = WeaponProficiency.Simple,
                    SavingThrowProficiencies = AbilityScore.Intelligence | AbilityScore.Wisdom,
                    SpellcastingAbility = AbilityScore.Wisdom,
                },

                // ── FIGHTER ──────────────────────────────────────────────────────────
                new Class
                {
                    ID = fighterId,
                    Name = "Fighter",
                    Description = """{"summary":"A master of martial combat skilled with a variety of weapons and armor, capable of remarkable feats of strength."}""",
                    HitDie = DieType.D10,
                    SkillProficienciesAmount = 2,
                    SubClassUnlockingLevel = 3,
                    ArmourProficiencies = ArmourProficiency.Light | ArmourProficiency.Medium | ArmourProficiency.Heavy | ArmourProficiency.Shield,
                    WeaponProficiencies = WeaponProficiency.Simple | WeaponProficiency.Martial,
                    SavingThrowProficiencies = AbilityScore.Strength | AbilityScore.Constitution,
                    SpellcastingAbility = null,
                },

                // ── MONK ─────────────────────────────────────────────────────────────
                new Class
                {
                    ID = monkId,
                    Name = "Monk",
                    Description = """{"summary":"A master of martial arts who harnesses the power of ki to perform incredible feats of speed, strength, and discipline."}""",
                    HitDie = DieType.D8,
                    SkillProficienciesAmount = 2,
                    SubClassUnlockingLevel = 3,
                    ArmourProficiencies = ArmourProficiency.None,
                    WeaponProficiencies = WeaponProficiency.Simple,
                    SavingThrowProficiencies = AbilityScore.Strength | AbilityScore.Dexterity,
                    SpellcastingAbility = null,
                },

                // ── PALADIN ──────────────────────────────────────────────────────────
                new Class
                {
                    ID = paladinId,
                    Name = "Paladin",
                    Description = """{"summary":"A holy warrior bound by a sacred oath, combining martial prowess with divine spells and auras to protect the innocent."}""",
                    HitDie = DieType.D10,
                    SkillProficienciesAmount = 2,
                    SubClassUnlockingLevel = 3,
                    ArmourProficiencies = ArmourProficiency.Light | ArmourProficiency.Medium | ArmourProficiency.Heavy | ArmourProficiency.Shield,
                    WeaponProficiencies = WeaponProficiency.Simple | WeaponProficiency.Martial,
                    SavingThrowProficiencies = AbilityScore.Wisdom | AbilityScore.Charisma,
                    SpellcastingAbility = AbilityScore.Charisma,
                },

                // ── RANGER ───────────────────────────────────────────────────────────
                new Class
                {
                    ID = rangerId,
                    Name = "Ranger",
                    Description = """{"summary":"A warrior who uses martial prowess and nature magic to combat threats on the edges of civilization."}""",
                    HitDie = DieType.D10,
                    SkillProficienciesAmount = 3,
                    SubClassUnlockingLevel = 3,
                    ArmourProficiencies = ArmourProficiency.Light | ArmourProficiency.Medium | ArmourProficiency.Shield,
                    WeaponProficiencies = WeaponProficiency.Simple | WeaponProficiency.Martial,
                    SavingThrowProficiencies = AbilityScore.Strength | AbilityScore.Dexterity,
                    SpellcastingAbility = AbilityScore.Wisdom,
                },

                // ── ROGUE ────────────────────────────────────────────────────────────
                new Class
                {
                    ID = rogueId,
                    Name = "Rogue",
                    Description = """{"summary":"A scoundrel who uses stealth and trickery to overcome obstacles and enemies, dealing lethal sneak attacks from the shadows."}""",
                    HitDie = DieType.D8,
                    SkillProficienciesAmount = 4,
                    SubClassUnlockingLevel = 3,
                    ArmourProficiencies = ArmourProficiency.Light,
                    WeaponProficiencies = WeaponProficiency.Simple,
                    SavingThrowProficiencies = AbilityScore.Dexterity | AbilityScore.Intelligence,
                    SpellcastingAbility = null,
                },

                // ── SORCERER ─────────────────────────────────────────────────────────
                new Class
                {
                    ID = sorcererId,
                    Name = "Sorcerer",
                    Description = """{"summary":"A spellcaster who draws on inherent magic from a gift or bloodline, shaping raw magical energy with Metamagic."}""",
                    HitDie = DieType.D6,
                    SkillProficienciesAmount = 2,
                    SubClassUnlockingLevel = 1,
                    ArmourProficiencies = ArmourProficiency.None,
                    WeaponProficiencies = WeaponProficiency.Simple,
                    SavingThrowProficiencies = AbilityScore.Constitution | AbilityScore.Charisma,
                    SpellcastingAbility = AbilityScore.Charisma,
                },

                // ── WARLOCK ──────────────────────────────────────────────────────────
                new Class
                {
                    ID = warlockId,
                    Name = "Warlock",
                    Description = """{"summary":"A wielder of magic derived from a bargain with an extraplanar entity, supplementing limited spell slots with eldritch invocations."}""",
                    HitDie = DieType.D8,
                    SkillProficienciesAmount = 2,
                    SubClassUnlockingLevel = 1,
                    ArmourProficiencies = ArmourProficiency.Light,
                    WeaponProficiencies = WeaponProficiency.Simple,
                    SavingThrowProficiencies = AbilityScore.Wisdom | AbilityScore.Charisma,
                    SpellcastingAbility = AbilityScore.Charisma,
                },

                // ── WIZARD ───────────────────────────────────────────────────────────
                new Class
                {
                    ID = wizardId,
                    Name = "Wizard",
                    Description = """{"summary":"A scholarly magic-user who learns spells through intense study and records them in a spellbook, commanding immense arcane power."}""",
                    HitDie = DieType.D6,
                    SkillProficienciesAmount = 2,
                    SubClassUnlockingLevel = 2,
                    ArmourProficiencies = ArmourProficiency.None,
                    WeaponProficiencies = WeaponProficiency.Simple,
                    SavingThrowProficiencies = AbilityScore.Intelligence | AbilityScore.Wisdom,
                    SpellcastingAbility = AbilityScore.Intelligence,
                },
            };

            context.Classes.AddRange(classes);

            // =========================================================================
            // SKILL POOLS
            // =========================================================================
            var skillPools = new List<ClassSkillPool>
            {
                // Barbarian
                new() { ClassID = barbarianId, Skill = Skill.AnimalHandling },
                new() { ClassID = barbarianId, Skill = Skill.Athletics },
                new() { ClassID = barbarianId, Skill = Skill.Intimidation },
                new() { ClassID = barbarianId, Skill = Skill.Nature },
                new() { ClassID = barbarianId, Skill = Skill.Perception },
                new() { ClassID = barbarianId, Skill = Skill.Survival },

                // Bard
                new() { ClassID = bardId, Skill = Skill.Acrobatics },
                new() { ClassID = bardId, Skill = Skill.AnimalHandling },
                new() { ClassID = bardId, Skill = Skill.Arcana },
                new() { ClassID = bardId, Skill = Skill.Athletics },
                new() { ClassID = bardId, Skill = Skill.Deception },
                new() { ClassID = bardId, Skill = Skill.History },
                new() { ClassID = bardId, Skill = Skill.Insight },
                new() { ClassID = bardId, Skill = Skill.Intimidation },
                new() { ClassID = bardId, Skill = Skill.Investigation },
                new() { ClassID = bardId, Skill = Skill.Medicine },
                new() { ClassID = bardId, Skill = Skill.Nature },
                new() { ClassID = bardId, Skill = Skill.Perception },
                new() { ClassID = bardId, Skill = Skill.Performance },
                new() { ClassID = bardId, Skill = Skill.Persuasion },
                new() { ClassID = bardId, Skill = Skill.Religion },
                new() { ClassID = bardId, Skill = Skill.SleightOfHand },
                new() { ClassID = bardId, Skill = Skill.Stealth },
                new() { ClassID = bardId, Skill = Skill.Survival },

                // Cleric
                new() { ClassID = clericId, Skill = Skill.History },
                new() { ClassID = clericId, Skill = Skill.Insight },
                new() { ClassID = clericId, Skill = Skill.Medicine },
                new() { ClassID = clericId, Skill = Skill.Persuasion },
                new() { ClassID = clericId, Skill = Skill.Religion },

                // Druid
                new() { ClassID = druidId, Skill = Skill.Arcana },
                new() { ClassID = druidId, Skill = Skill.AnimalHandling },
                new() { ClassID = druidId, Skill = Skill.Insight },
                new() { ClassID = druidId, Skill = Skill.Medicine },
                new() { ClassID = druidId, Skill = Skill.Nature },
                new() { ClassID = druidId, Skill = Skill.Perception },
                new() { ClassID = druidId, Skill = Skill.Religion },
                new() { ClassID = druidId, Skill = Skill.Survival },

                // Fighter
                new() { ClassID = fighterId, Skill = Skill.Acrobatics },
                new() { ClassID = fighterId, Skill = Skill.AnimalHandling },
                new() { ClassID = fighterId, Skill = Skill.Athletics },
                new() { ClassID = fighterId, Skill = Skill.History },
                new() { ClassID = fighterId, Skill = Skill.Insight },
                new() { ClassID = fighterId, Skill = Skill.Intimidation },
                new() { ClassID = fighterId, Skill = Skill.Perception },
                new() { ClassID = fighterId, Skill = Skill.Survival },

                // Monk
                new() { ClassID = monkId, Skill = Skill.Acrobatics },
                new() { ClassID = monkId, Skill = Skill.Athletics },
                new() { ClassID = monkId, Skill = Skill.History },
                new() { ClassID = monkId, Skill = Skill.Insight },
                new() { ClassID = monkId, Skill = Skill.Religion },
                new() { ClassID = monkId, Skill = Skill.Stealth },

                // Paladin
                new() { ClassID = paladinId, Skill = Skill.Athletics },
                new() { ClassID = paladinId, Skill = Skill.Insight },
                new() { ClassID = paladinId, Skill = Skill.Intimidation },
                new() { ClassID = paladinId, Skill = Skill.Medicine },
                new() { ClassID = paladinId, Skill = Skill.Persuasion },
                new() { ClassID = paladinId, Skill = Skill.Religion },

                // Ranger
                new() { ClassID = rangerId, Skill = Skill.AnimalHandling },
                new() { ClassID = rangerId, Skill = Skill.Athletics },
                new() { ClassID = rangerId, Skill = Skill.Insight },
                new() { ClassID = rangerId, Skill = Skill.Investigation },
                new() { ClassID = rangerId, Skill = Skill.Nature },
                new() { ClassID = rangerId, Skill = Skill.Perception },
                new() { ClassID = rangerId, Skill = Skill.Stealth },
                new() { ClassID = rangerId, Skill = Skill.Survival },

                // Rogue
                new() { ClassID = rogueId, Skill = Skill.Acrobatics },
                new() { ClassID = rogueId, Skill = Skill.Athletics },
                new() { ClassID = rogueId, Skill = Skill.Deception },
                new() { ClassID = rogueId, Skill = Skill.Insight },
                new() { ClassID = rogueId, Skill = Skill.Intimidation },
                new() { ClassID = rogueId, Skill = Skill.Investigation },
                new() { ClassID = rogueId, Skill = Skill.Perception },
                new() { ClassID = rogueId, Skill = Skill.Performance },
                new() { ClassID = rogueId, Skill = Skill.Persuasion },
                new() { ClassID = rogueId, Skill = Skill.SleightOfHand },
                new() { ClassID = rogueId, Skill = Skill.Stealth },

                // Sorcerer
                new() { ClassID = sorcererId, Skill = Skill.Arcana },
                new() { ClassID = sorcererId, Skill = Skill.Deception },
                new() { ClassID = sorcererId, Skill = Skill.Insight },
                new() { ClassID = sorcererId, Skill = Skill.Intimidation },
                new() { ClassID = sorcererId, Skill = Skill.Persuasion },
                new() { ClassID = sorcererId, Skill = Skill.Religion },

                // Warlock
                new() { ClassID = warlockId, Skill = Skill.Arcana },
                new() { ClassID = warlockId, Skill = Skill.Deception },
                new() { ClassID = warlockId, Skill = Skill.History },
                new() { ClassID = warlockId, Skill = Skill.Intimidation },
                new() { ClassID = warlockId, Skill = Skill.Investigation },
                new() { ClassID = warlockId, Skill = Skill.Nature },
                new() { ClassID = warlockId, Skill = Skill.Religion },

                // Wizard
                new() { ClassID = wizardId, Skill = Skill.Arcana },
                new() { ClassID = wizardId, Skill = Skill.History },
                new() { ClassID = wizardId, Skill = Skill.Insight },
                new() { ClassID = wizardId, Skill = Skill.Investigation },
                new() { ClassID = wizardId, Skill = Skill.Medicine },
                new() { ClassID = wizardId, Skill = Skill.Religion },
            };

            context.ClassSkillPools.AddRange(skillPools);

            // =========================================================================
            // SPELL SLOT PROGRESSIONS
            // =========================================================================

            // Helper: full caster table (Bard, Cleric, Druid, Sorcerer, Wizard)
            // [level] = { s1, s2, s3, s4, s5, s6, s7, s8, s9 }
            int[][] fullCasterSlots =
            {
                //       s1  s2  s3  s4  s5  s6  s7  s8  s9
                new[] {  2,  0,  0,  0,  0,  0,  0,  0,  0 }, // 1
                new[] {  3,  0,  0,  0,  0,  0,  0,  0,  0 }, // 2
                new[] {  4,  2,  0,  0,  0,  0,  0,  0,  0 }, // 3
                new[] {  4,  3,  0,  0,  0,  0,  0,  0,  0 }, // 4
                new[] {  4,  3,  2,  0,  0,  0,  0,  0,  0 }, // 5
                new[] {  4,  3,  3,  0,  0,  0,  0,  0,  0 }, // 6
                new[] {  4,  3,  3,  1,  0,  0,  0,  0,  0 }, // 7
                new[] {  4,  3,  3,  2,  0,  0,  0,  0,  0 }, // 8
                new[] {  4,  3,  3,  3,  1,  0,  0,  0,  0 }, // 9
                new[] {  4,  3,  3,  3,  2,  0,  0,  0,  0 }, // 10
                new[] {  4,  3,  3,  3,  2,  1,  0,  0,  0 }, // 11
                new[] {  4,  3,  3,  3,  2,  1,  0,  0,  0 }, // 12
                new[] {  4,  3,  3,  3,  2,  1,  1,  0,  0 }, // 13
                new[] {  4,  3,  3,  3,  2,  1,  1,  0,  0 }, // 14
                new[] {  4,  3,  3,  3,  2,  1,  1,  1,  0 }, // 15
                new[] {  4,  3,  3,  3,  2,  1,  1,  1,  0 }, // 16
                new[] {  4,  3,  3,  3,  2,  1,  1,  1,  1 }, // 17
                new[] {  4,  3,  3,  3,  3,  1,  1,  1,  1 }, // 18
                new[] {  4,  3,  3,  3,  3,  2,  1,  1,  1 }, // 19
                new[] {  4,  3,  3,  3,  3,  2,  2,  1,  1 }, // 20
            };

            // Half caster table (Paladin, Ranger) — no slots at level 1
            int[][] halfCasterSlots =
            {
                new[] {  0,  0,  0,  0,  0,  0,  0,  0,  0 }, // 1
                new[] {  2,  0,  0,  0,  0,  0,  0,  0,  0 }, // 2
                new[] {  3,  0,  0,  0,  0,  0,  0,  0,  0 }, // 3
                new[] {  3,  0,  0,  0,  0,  0,  0,  0,  0 }, // 4
                new[] {  4,  2,  0,  0,  0,  0,  0,  0,  0 }, // 5
                new[] {  4,  2,  0,  0,  0,  0,  0,  0,  0 }, // 6
                new[] {  4,  3,  0,  0,  0,  0,  0,  0,  0 }, // 7
                new[] {  4,  3,  0,  0,  0,  0,  0,  0,  0 }, // 8
                new[] {  4,  3,  2,  0,  0,  0,  0,  0,  0 }, // 9
                new[] {  4,  3,  2,  0,  0,  0,  0,  0,  0 }, // 10
                new[] {  4,  3,  3,  0,  0,  0,  0,  0,  0 }, // 11
                new[] {  4,  3,  3,  0,  0,  0,  0,  0,  0 }, // 12
                new[] {  4,  3,  3,  1,  0,  0,  0,  0,  0 }, // 13
                new[] {  4,  3,  3,  1,  0,  0,  0,  0,  0 }, // 14
                new[] {  4,  3,  3,  2,  0,  0,  0,  0,  0 }, // 15
                new[] {  4,  3,  3,  2,  0,  0,  0,  0,  0 }, // 16
                new[] {  4,  3,  3,  3,  1,  0,  0,  0,  0 }, // 17
                new[] {  4,  3,  3,  3,  1,  0,  0,  0,  0 }, // 18
                new[] {  4,  3,  3,  3,  2,  0,  0,  0,  0 }, // 19
                new[] {  4,  3,  3,  3,  2,  0,  0,  0,  0 }, // 20
            };

            // Warlock pact slots (short-rest recharge)
            // [level] = { pactSlotLevel, pactSlotCount }
            (int level, int count)[] warlockPactSlots =
            {
                (1, 1), (1, 2), (2, 2), (2, 2), (3, 2),
                (3, 2), (4, 2), (4, 2), (5, 2), (5, 2),
                (5, 3), (5, 3), (5, 3), (5, 3), (5, 3),
                (5, 3), (5, 4), (5, 4), (5, 4), (5, 4),
            };

            // Sorcerer spells known
            int[] sorcererSpellsKnown = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 12, 13, 13, 14, 14, 15, 15, 15, 15 };
            int[] sorcererCantrips = { 4, 4, 4, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6 };

            // Bard spells known
            int[] bardSpellsKnown = { 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15, 15, 16, 18, 19, 19, 20, 22, 22, 22 };
            int[] bardCantrips = { 2, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 };

            // Warlock spells known
            int[] warlockSpellsKnown = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15 };
            int[] warlockCantrips = { 2, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 };

            // Wizard cantrips known
            int[] wizardCantrips = { 3, 3, 3, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 };

            // Cleric cantrips known
            int[] clericCantrips = { 3, 3, 3, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 };

            // Druid cantrips known
            int[] druidCantrips = { 2, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 };

            var spellSlots = new List<ClassSpellSlotProgression>();

            ClassSpellSlotProgression MakeSlot(Guid classId, int level, int[] slots,
                int? cantrips = null, int? spellsKnown = null,
                int? pactLevel = null, int? pactCount = null) =>
                new()
                {
                    ClassID = classId,
                    ClassLevel = level,
                    Slot1 = slots[0],
                    Slot2 = slots[1],
                    Slot3 = slots[2],
                    Slot4 = slots[3],
                    Slot5 = slots[4],
                    Slot6 = slots[5],
                    Slot7 = slots[6],
                    Slot8 = slots[7],
                    Slot9 = slots[8],
                    CantripsKnown = cantrips,
                    SpellsKnown = spellsKnown,
                    PactSlotLevel = pactLevel,
                    PactSlotCount = pactCount,
                };

            // Bard
            for (int i = 0; i < 20; i++)
                spellSlots.Add(MakeSlot(bardId, i + 1, fullCasterSlots[i],
                    cantrips: bardCantrips[i], spellsKnown: bardSpellsKnown[i]));

            // Cleric
            for (int i = 0; i < 20; i++)
                spellSlots.Add(MakeSlot(clericId, i + 1, fullCasterSlots[i],
                    cantrips: clericCantrips[i]));

            // Druid
            for (int i = 0; i < 20; i++)
                spellSlots.Add(MakeSlot(druidId, i + 1, fullCasterSlots[i],
                    cantrips: druidCantrips[i]));

            // Paladin
            for (int i = 0; i < 20; i++)
                spellSlots.Add(MakeSlot(paladinId, i + 1, halfCasterSlots[i]));

            // Ranger
            for (int i = 0; i < 20; i++)
                spellSlots.Add(MakeSlot(rangerId, i + 1, halfCasterSlots[i]));

            // Sorcerer
            for (int i = 0; i < 20; i++)
                spellSlots.Add(MakeSlot(sorcererId, i + 1, fullCasterSlots[i],
                    cantrips: sorcererCantrips[i], spellsKnown: sorcererSpellsKnown[i]));

            // Warlock (no standard slots — only pact slots)
            var emptySlots = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 20; i++)
                spellSlots.Add(MakeSlot(warlockId, i + 1, emptySlots,
                    cantrips: warlockCantrips[i], spellsKnown: warlockSpellsKnown[i],
                    pactLevel: warlockPactSlots[i].level, pactCount: warlockPactSlots[i].count));

            // Wizard
            for (int i = 0; i < 20; i++)
                spellSlots.Add(MakeSlot(wizardId, i + 1, fullCasterSlots[i],
                    cantrips: wizardCantrips[i]));

            // Non-casters: Barbarian, Fighter, Monk, Rogue — no spell slot rows needed.
            // Add empty rows only if your app expects a row per level per class:
            // for (int i = 0; i < 20; i++)
            //     spellSlots.Add(MakeSlot(barbarianId, i + 1, emptySlots));

            context.ClassSpellSlotProgressions.AddRange(spellSlots);

            context.SaveChanges();
        }
    }
}
