//////////////////////////////////////////////////
//           Battleground/Combat.cs             //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using Styx.TreeSharp;

namespace RogueBT.Composites.Context.Battleground
{
    class Combat
    {
        // For now, just use the same behavior as our level context.

        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(
                Level.Combat.BuildCombatBehavior()
            );
        }

        static public Composite BuildPullBehavior()
        {
            return Level.Combat.BuildPullBehavior();
        }

        static public Composite BuildBuffBehavior()
        {
            return Level.Combat.BuildBuffBehavior();
        }
    }
}
