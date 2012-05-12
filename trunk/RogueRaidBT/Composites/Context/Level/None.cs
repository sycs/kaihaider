//////////////////////////////////////////////////
//               Level/None.cs                  //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////


using TreeSharp;

namespace RogueRaidBT.Composites.Context.Level
{
    static class None
    {
        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(
                Helpers.Movement.MoveToTarget(),

                Helpers.Spells.ToggleAutoAttack(ret => !Helpers.Aura.Vanish && !Helpers.Aura.IsTargetDisoriented && !Helpers.Aura.IsTargetSapped),

                Helpers.Spells.CastSelf("Evasion", ret => Helpers.Rogue.mHP <= 35),

                Helpers.Spells.Cast("Eviscerate", ret => Helpers.Rogue.mComboPoints == 5 || Helpers.Rogue.mTargetHP <= 60),
                Helpers.Spells.Cast("Sinister Strike")
            );
        }

        static public Composite BuildPullBehavior()
        {
            return new PrioritySelector(
                Helpers.Movement.MoveToTarget(),
                Helpers.Spells.Cast("Sinister Strike")
            );
        }
    }
}
