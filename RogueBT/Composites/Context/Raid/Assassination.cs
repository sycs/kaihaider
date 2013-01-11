//////////////////////////////////////////////////
//            Raid/Assassination.cs             //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Linq;
using CommonBehaviors.Actions;
using Styx;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;

namespace RogueBT.Composites.Context.Raid
{

    static class Assassination
    {
        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(
                Helpers.Movement.PleaseStop(),
                //Helpers.Target.EnsureValidTarget(),
                Helpers.Movement.MoveToLos(),
                //Helpers.Movement.ChkFace(),
                Helpers.Spells.ToggleAutoAttack(),

                Helpers.Spells.CastCooldown("Feint", ret => (Helpers.Aura.IsTargetCasting == 109034 || Helpers.Aura.IsTargetCasting == 109033) 
                    && Helpers.Movement.IsInSafeMeleeRange),



                Helpers.Spells.CastSelf("Slice and Dice", ret => Helpers.Aura.TimeSliceandDice < 2 &&
                                                                 Helpers.Rogue.mComboPoints >= 1),

                Helpers.Spells.Cast("Rupture", ret => ((Helpers.Aura.TimeRupture < 1 &&
                                                                 Helpers.Rogue.mCurrentEnergy <= 100) ||
                                                                 !Helpers.Aura.Rupture) &&
                                                                 Helpers.Rogue.mComboPoints > 4 && Helpers.Movement.IsInSafeMeleeRange),


                Helpers.Spells.Cast("Crimson Tempest", ret => Helpers.Rogue.IsAoeUsable() && Helpers.Rogue.mComboPoints > 4 &&
                                                            Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 2),

                Helpers.Spells.Cast("Envenom", ret => Helpers.Aura.DeadlyPoison && Helpers.Aura.FuryoftheDestroyer && Helpers.Movement.IsInSafeMeleeRange),

                new Decorator(ret => Helpers.Rogue.IsCooldownsUsable() &&
                                     Helpers.Aura.SliceandDice &&
                                     Helpers.Aura.Rupture,

                    new PrioritySelector(
                        Helpers.Specials.UseSpecialAbilities(ret => Helpers.Aura.Vendetta ||
                                                                    Helpers.Aura.TimeVendetta > 0),
                        Helpers.Spells.CastCooldown("Vendetta"),

                        new Decorator(ret => Helpers.Spells.CanCast("Vanish")  &&
                                             Helpers.Rogue.mCurrentEnergy >= 60 && Helpers.Rogue.mCurrentEnergy <= 100 && 
                                             Helpers.Rogue.mComboPoints != 5,
                            new Sequence(
                                Helpers.Spells.CastSelf("Vanish"),
                                Helpers.Rogue.CreateWaitForLagDuration(),
                                Helpers.Spells.Cast("Garrote", ret => Helpers.Movement.IsInSafeMeleeRange)
                                )
                            )
                    )
                ),

                Helpers.Spells.Cast("Envenom", ret => Helpers.Aura.DeadlyPoison &&
                                (((Helpers.Rogue.mCurrentEnergy >= 90 && Helpers.Rogue.mComboPoints >= 4 && Helpers.Rogue.mTargetHP >= 35) ||
                                                       (Helpers.Rogue.mCurrentEnergy >= 55 && Helpers.Rogue.mComboPoints == 5 && Helpers.Rogue.mTargetHP < 35) ||
                                                       (Helpers.Aura.TimeSliceandDice <= 3 && Helpers.Rogue.mComboPoints >= 1)) &&
                                                       (!Helpers.Aura.Envenom || Helpers.Rogue.mCurrentEnergy > 100)) && Helpers.Movement.IsInSafeMeleeRange),

                
                Helpers.Spells.Cast("Fan of Knives", ret => Helpers.Rogue.IsAoeUsable() 
                                                            && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 1),

                Helpers.Spells.Cast("Dispatch", ret => (Helpers.Rogue.mTargetHP < 35 || Helpers.Aura.Blindside) 
                                                        && ((!Helpers.Aura.Rupture || (Helpers.Rogue.mCurrentEnergy >= 80 || Helpers.Aura.Envenom))) 
                                                       && Helpers.Movement.IsInSafeMeleeRange),

                Helpers.Spells.Cast("Mutilate", ret => !Helpers.Aura.Rupture ||
                                                       (Helpers.Rogue.mComboPoints < 4 && (Helpers.Rogue.mCurrentEnergy >= 90 ||
                                                       Helpers.Aura.Envenom)) && Helpers.Movement.IsInSafeMeleeRange),

                                                       

                Helpers.Spells.CastFocus("Tricks of the Trade", ret => !Helpers.Aura.Tricks  && Helpers.Focus.mFocusTarget!=null &&
                                                                       Helpers.Rogue.mCurrentEnergy > 50 && Helpers.Rogue.mCurrentEnergy < 95 
                                                                       && Helpers.Rogue.mComboPoints > 1),


                Helpers.Movement.MoveToTarget(),

                Helpers.Spells.Cast("Redirect", ret => Helpers.Rogue.mComboPoints < Helpers.Rogue.me.RawComboPoints)
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
