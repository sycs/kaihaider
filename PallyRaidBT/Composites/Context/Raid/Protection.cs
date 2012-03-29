//////////////////////////////////////////////////
//               Raid/Protection.cs            //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from PallyRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////
using CommonBehaviors.Actions;
using Styx;
using TreeSharp;

namespace PallyRaidBT.Composites.Context.Raid
{
    class Protection
    {
        static public Composite BuildCombatBehavior()
        {
            return Raid.Retribution.BuildCombatBehavior();
        }

        static public Composite BuildPullBehavior()
        {
            return Raid.Retribution.BuildPullBehavior();
        }

        static public Composite BuildBuffBehavior()
        {
            return Raid.Retribution.BuildBuffBehavior();
        }
    }
}
