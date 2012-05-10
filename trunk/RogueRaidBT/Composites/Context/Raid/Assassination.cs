//////////////////////////////////////////////////
//            Raid/Assassination.cs             //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Linq;
using CommonBehaviors.Actions;
using Styx;
using TreeSharp;

namespace RogueRaidBT.Composites.Context.Raid
{

    static class Assassination
    {
        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(
                Helpers.Spells.ToggleAutoAttack(ret => !Helpers.Aura.Vanish && !Helpers.Aura.IsTargetDisoriented && !Helpers.Aura.IsTargetSapped),


                Helpers.Spells.Cast("Redirect", ret => Helpers.Rogue.mComboPoints < Helpers.Rogue.mRawComboPoints),

                Helpers.Spells.CastCooldown("Feint", ret => (Helpers.Aura.IsTargetCasting == 109034 || Helpers.Aura.IsTargetCasting == 109033 )&& 
                    Helpers.Rogue.mTarget.IsWithinMeleeRange),

                Helpers.Spells.Cast("Envenom", ret => Helpers.Aura.DeadlyPoison && Helpers.Aura.FuryoftheDestroyer),

                Helpers.Spells.CastSelf("Slice and Dice", ret => !Helpers.Aura.SliceandDice &&
                                                                 Helpers.Rogue.mComboPoints >= 1),

                Helpers.Spells.Cast("Rupture", ret => ((Helpers.Aura.TimeRupture < 1 &&
                                                                 Helpers.Rogue.mCurrentEnergy <= 100) ||
                                                                 !Helpers.Aura.Rupture) &&
                                                                 Helpers.Rogue.mComboPoints >= 1),

                Helpers.Spells.CastFocus("Tricks of the Trade", ret => !Helpers.Aura.Tricks  && Helpers.Focus.mFocusTarget!=null &&
                                                                       Helpers.Rogue.mCurrentEnergy > 50 && Helpers.Rogue.mComboPoints > 3),

                Helpers.Spells.Cast("Fan of Knives", ret => Helpers.Rogue.IsAoeUsable() &&
                                                            Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 3),

                new Decorator(ret => Helpers.Rogue.IsCooldownsUsable() &&
                                     Helpers.Aura.SliceandDice &&
                                     Helpers.Aura.Rupture,

                    new PrioritySelector(
                        Helpers.Specials.UseSpecialAbilities(ret => Helpers.Aura.Vendetta ||
                                                                    Helpers.Aura.TimeVendetta > 0),
                        Helpers.Spells.CastCooldown("Vendetta"),

                        new Decorator(ret => Helpers.Spells.CanCast("Vanish") && !Helpers.Aura.Overkill &&
                                             Helpers.Rogue.mCurrentEnergy >= 60 && Helpers.Rogue.mCurrentEnergy <= 100 && 
                                             Helpers.Rogue.mComboPoints != 5,
                            new Sequence(
                                Helpers.Spells.CastSelf("Vanish"),
                                new WaitContinue(1, ret => false, new ActionAlwaysSucceed()),//!stealth
                                Helpers.Spells.Cast("Garrote")
                                )
                            ),

                        Helpers.Spells.CastSelf("Cold Blood", ret => !Helpers.Aura.ColdBlood && Helpers.Rogue.mComboPoints == 5 &&
                                                                     Helpers.Rogue.mCurrentEnergy >= 60 && Helpers.Rogue.mCurrentEnergy <= 80)
                    )
                ),

                Helpers.Spells.Cast("Envenom", ret => Helpers.Aura.DeadlyPoison &&
                                (((Helpers.Rogue.mCurrentEnergy >= 90 && Helpers.Rogue.mComboPoints >= 4 && Helpers.Rogue.mTargetHP >= 35) ||
                                                       (Helpers.Rogue.mCurrentEnergy >= 55 && Helpers.Rogue.mComboPoints == 5 && Helpers.Rogue.mTargetHP < 35) ||
                                                       (Helpers.Aura.TimeSliceandDice <= 3 && Helpers.Rogue.mComboPoints >= 1)) &&
                                                       (!Helpers.Aura.Envenom || Helpers.Rogue.mCurrentEnergy > 100))),

                Helpers.Spells.Cast("Backstab", ret => Helpers.Rogue.IsBehindUnit(StyxWoW.Me.CurrentTarget) && Helpers.Rogue.mTargetHP < 35 &&
                                                       ((!Helpers.Aura.Rupture ||
                                                       (Helpers.Rogue.mComboPoints != 5 && Helpers.Rogue.mCurrentEnergy < 110 &&
                                                       (Helpers.Rogue.mCurrentEnergy >= 80 || Helpers.Aura.Envenom))))),

                Helpers.Spells.Cast("Mutilate", ret => !Helpers.Aura.Rupture ||
                                                       (Helpers.Rogue.mComboPoints < 4 && (Helpers.Rogue.mCurrentEnergy >= 90 ||
                                                       Helpers.Aura.Envenom)))

            );
        }

        static public Composite BuildPullBehavior()
        {
            return new Action(ret => RunStatus.Failure);
        }

        static public Composite BuildBuffBehavior()
        {
            return new Action(ret => RunStatus.Failure);
        }
    }
}
