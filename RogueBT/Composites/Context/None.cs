//////////////////////////////////////////////////
//                  None.cs                     //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using Styx.TreeSharp;

namespace RogueBT.Composites.Context
{
    static class None
    {
        static public Composite BuildCombatBehavior()
        {
            return new Decorator(ret => Settings.Mode.mUseCombat,
                Level.None.BuildCombatBehavior()
            );
        }

        static public Composite BuildPullBehavior()
        {
            return new Decorator(ret => Settings.Mode.mUseCombat,
                Level.None.BuildPullBehavior()
            );
        }

        static public Composite BuildBuffBehavior()
        {
            return new Decorator(ret => Settings.Mode.mUseCombat,
                Level.None.BuildBuffBehavior()
            );
        }
    }
}
