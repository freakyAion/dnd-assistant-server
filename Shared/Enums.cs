namespace dnd_assistant.Shared
{
    public class Enums
    {
        public enum DieType
        {
            D2 = 2,
            D4 = 4,
            D6 = 6,
            D8 = 8,
            D10 = 10,
            D12 = 12,
            D20 = 20,
            D100 = 100
        }

        public enum Alignment
        {
            LawfulGood,
            NeutralGood,
            ChaoticGood,
            LawfulNeutral,
            TrueNeutral,
            ChaoticNeutral,
            LawfulEvil,
            NeutralEvil,
            ChaoticEvil
        }

        [Flags]
        public enum ArmourProficiency
        {
            None = 0,
            Light = 1,
            Medium = 2,
            Heavy = 4,
            Shield = 8
        }

        [Flags]
        public enum WeaponProficiency
        {
            None = 0,
            Simple = 1,
            Martial = 2
        }

        [Flags]
        public enum AbilityScore
        {
            None = 0,
            Strength = 1,
            Dexterity = 2,
            Constitution = 4,
            Intelligence = 8,
            Wisdom = 16,
            Charisma = 32
        }

        public enum Skill
        {
            Acrobatics,
            AnimalHandling,
            Arcana,
            Athletics,
            Deception,
            History,
            Insight,
            Intimidation,
            Investigation,
            Medicine,
            Nature,
            Perception,
            Performance,
            Persuasion,
            Religion,
            SleightOfHand,
            Stealth,
            Survival
        }

        public enum AccessRole
        {
            Spectator = 0,
            Player = 1,
            GM = 2
        }
    }
}
