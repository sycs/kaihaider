//////////////////////////////////////////////////
//                  None.cs                     //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using TreeSharp;

namespace RogueRaidBT.Composites.Context
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
    }
}
