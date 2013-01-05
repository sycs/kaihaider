//////////////////////////////////////////////////
//       Battleground/Assassination.cs          //
//      Part of MutaRaidBT by fiftypence        //
//////////////////////////////////////////////////

using Styx.TreeSharp;

namespace RogueRaidBT.Composites.Context.Battleground
{
    static class Assassination
    {
        // For now, just use the same behavior as our level context.

        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(
                Level.Assassination.BuildCombatBehavior()
            );
        }

        static public Composite BuildPullBehavior()
        {
            return Level.Assassination.BuildPullBehavior();
        }

        static public Composite BuildBuffBehavior()
        {
            return Level.Assassination.BuildBuffBehavior();
        }
    }
}
