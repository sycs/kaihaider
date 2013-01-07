//////////////////////////////////////////////////
//                  Enum.cs                     //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

namespace RogueBT.Helpers
{
    class Enum
    {
        public enum LeathalPoisonSpellId
        {
            Deadly = 2823,
            Wound = 8679
        }

        public enum NonLeathalPoisonSpellId
        {
            
            MindNumbing = 5761,
            Crippling = 3408,
            Paralytic = 108215,
            Leeching = 108211
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
