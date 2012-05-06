//////////////////////////////////////////////////
//                  Enum.cs                     //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

namespace RogueRaidBT.Helpers
{
    class Enum
    {
        public enum PoisonSpellId
        {
            Deadly = 2892,
            Instant = 6947,
            Crippling = 3775,
            Wound = 10918,
            MindNumbing = 5237
        }

        public enum TalentTrees
        {
            None = 0,
            Assassination,
            Combat,
            Subtlety
        }

        public enum LocationContext
        {
            Undefined = 0,
            Raid,
            Arena,
            Dungeon,
            Battleground,
            World,
            HeroicDungeon
        }

        public enum CooldownUse
        {
            Always = 0,
            ByFocus,
            OnlyOnBosses,
            Never
        }
    }
}
