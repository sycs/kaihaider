//////////////////////////////////////////////////
//                 Enumeration.cs               //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////
 
namespace PallyRaidBT.Helpers
{
    class Enumeration
    {
        

        public enum TalentTrees
        {
            None = 0,
            Retribution,
            Holy,
            Protection
        }

        public enum LocationContext
        {
            Undefined = 0,
            Raid,
            HeroicDungeon,
            Dungeon,
            Battleground,
            World,
            Arena
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
