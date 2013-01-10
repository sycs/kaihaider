//////////////////////////////////////////////////
//               Level/None.cs                  //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////


using Styx.TreeSharp;

namespace RogueBT.Composites.Context.Level
{
    static class None
    {
        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(
               Helpers.Movement.PleaseStop(),
                //Helpers.Target.EnsureValidTarget(),
                Helpers.Movement.MoveToLos(),
                //Helpers.Movement.ChkFace(),
                Helpers.Spells.ToggleAutoAttack(),
                

                Helpers.Spells.CastSelf("Evasion", ret => Helpers.Rogue.mHP <= 35 && Helpers.Movement.IsInSafeMeleeRange),

                Helpers.Spells.Cast("Eviscerate", ret => (Helpers.Rogue.mComboPoints == 5 || Helpers.Rogue.mComboPoints > 0 && Helpers.Rogue.mTargetHP <= 60) 
                        && Helpers.Movement.IsInSafeMeleeRange),
                Helpers.Spells.Cast("Sinister Strike", ret =>  Helpers.Movement.IsInSafeMeleeRange),
                
                Helpers.Movement.MoveToTarget()
            );
        }

        static public Composite BuildPullBehavior()
        {
            return new PrioritySelector(
                Helpers.Movement.MoveToLos(),
                Helpers.Spells.Cast("Sinister Strike", ret => Helpers.General.UpdateHelpersBool() && Helpers.Movement.IsInSafeMeleeRange),
                Helpers.Movement.PullMoveToTarget()
            );
        }


        static public Composite BuildBuffBehavior()
        {
            return new Decorator(ret => false,
                new PrioritySelector(
                    Helpers.Spells.CastSelf("Recuperate", ret => !Helpers.Spells.IsAuraActive(Helpers.Rogue.me, "Recuperate") &&
                                                                     Helpers.Rogue.mRawComboPoints >= 1 && Helpers.Rogue.CheckSpamLock()),
                    Helpers.Spells.CastSelf("Slice and Dice", ret => !Helpers.Spells.IsAuraActive(Helpers.Rogue.me, "Slice and Dice") &&
                                                                     Helpers.Rogue.mRawComboPoints >= 1 && Helpers.Rogue.CheckSpamLock())
                )
            );
        }
    }
}
