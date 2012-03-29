//////////////////////////////////////////////////
//                       None.cs                //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from PallyRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////
using Styx;
using TreeSharp;

namespace PallyRaidBT.Composites.Context
{
    class None
    {
        static public Composite BuildCombatBehavior()
        {
            return new Decorator(ret => StyxWoW.Me.CurrentTarget != null && Settings.Mode.mUseCombat,
                Level.None.BuildCombatBehavior()
            );
        }

        static public Composite BuildPullBehavior()
        {
            return new Decorator(ret => StyxWoW.Me.CurrentTarget != null && Settings.Mode.mUseCombat,
                Level.None.BuildPullBehavior()
            );
        }
    }
}
