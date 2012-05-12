//////////////////////////////////////////////////
//           Battleground/Combat.cs             //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using TreeSharp;

namespace RogueRaidBT.Composites.Context.Battleground
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
