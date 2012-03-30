//////////////////////////////////////////////////
//           Battleground/Protection .cs        //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using TreeSharp;
namespace PallyRaidBT.Composites.Context.Battleground
{
    class Protection
    {
        static public Composite BuildCombatBehavior()
        {
            return Raid.Protection.BuildCombatBehavior();
        }

        static public Composite BuildPullBehavior()
        {
            return Raid.Protection.BuildPullBehavior();
        }

        static public Composite BuildBuffBehavior()
        {
            return Raid.Protection.BuildBuffBehavior();
        }
    }
}
