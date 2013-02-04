﻿//////////////////////////////////////////////////
//              Raid/Subtlety.cs                //
//        Part of RogueBT by kaihaider          //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System;
using System.Linq;
using CommonBehaviors.Actions;
using Styx;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;

namespace RogueBT.Composites.Context.Raid
{
    static class Subtlety
    {
        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(
                Helpers.Movement.PleaseStop(),
                //Helpers.Target.EnsureValidTarget(),
                //Helpers.Movement.MoveToLos(),
                //Helpers.Movement.ChkFace(),
                Helpers.Spells.ToggleAutoAttack(),
                Helpers.Movement.MoveToTarget(),

                Helpers.Spells.CastCooldown("Feint", ret => (Helpers.Aura.IsTargetCasting == 109034 || Helpers.Aura.IsTargetCasting == 109033) &&
                    Helpers.Movement.IsInSafeMeleeRange),

                Helpers.Rogue.TryToInterrupt(ret => Helpers.Rogue.mTarget.CanInterruptCurrentSpellCast && Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds <= 0.6 &&
                    Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds >= 0.2 ),

                new Decorator(ret => Helpers.Rogue.mComboPoints == 5 || Helpers.Aura.FuryoftheDestroyer,
                    new PrioritySelector(

                        Helpers.Spells.CastSelf("Slice and Dice", ret => Helpers.Aura.TimeSliceandDice < 3),

                        Helpers.Spells.Cast("Crimson Tempest", ret => Helpers.Rogue.IsAoeUsable() &&
                                                            Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 2),

                        Helpers.Spells.Cast("Rupture", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                                    Helpers.Aura.TimeRupture < 3 && (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)),
                //!Helpers.Aura.Sanguinary Vein 
                        Helpers.Spells.Cast("Eviscerate", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish)
                                        && (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement))
                    )

                ),

                Helpers.Spells.CastCooldown("Premeditation", ret => Helpers.Rogue.mComboPoints <= 3 && (Helpers.Aura.Stealth || 
                                                                    Helpers.Aura.ShadowDance || Helpers.Aura.Vanish)),

                Helpers.Specials.UseSpecialAbilities(ret => Helpers.Aura.ShadowDance ||
                                                            Helpers.Spells.GetSpellCooldown("Shadow Dance") >= 10),

                new Decorator(ret => Helpers.Rogue.IsCooldownsUsable()
                            && !Helpers.Aura.ShadowDance && !Helpers.Aura.FindWeakness
                            && Helpers.Aura.TimeRupture > 10 && Helpers.Aura.TimeSliceandDice > 10
                                     && Helpers.Rogue.mComboPoints < 3
                                     && Helpers.Rogue.mCurrentEnergy >= 50
                                     && !(Helpers.Spells.GetSpellCooldown("Premeditation") > 0),
                    new PrioritySelector(
                        new Decorator(ret => Helpers.Spells.CanCast("Shadow Dance"),
                            new Sequence(
                                Helpers.Spells.CastSelf("Shadow Dance"),
                                Helpers.Rogue.CreateWaitForLagDuration()
                            )
                        ),

                        new Decorator(ret => !Helpers.Aura.ShadowDance && !Helpers.Aura.FindWeakness && (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)
                            && Helpers.Aura.TimeRupture > 16 && Helpers.Aura.TimeSliceandDice > 16
                                             && Helpers.Spells.GetSpellCooldown("Shadow Dance") > 0
                                             && Helpers.Spells.CanCast("Vanish"),
                            new Sequence(
                                Helpers.Spells.CastSelf("Vanish"),
                                Helpers.Rogue.CreateWaitForLagDuration(),
                                Helpers.Spells.CastCooldown("Premeditation", ret => Helpers.Aura.ShadowDance || Helpers.Aura.Stealth || Helpers.Aura.Vanish),
                                Helpers.Movement.MoveToTarget(),
                                Helpers.Spells.Cast("Ambush", ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Aura.IsBehind)
                            )
                        ),
                        Helpers.Spells.CastSelf("Shadow Blades", ret => !Helpers.Aura.ShadowDance && !Helpers.Aura.FindWeakness)
                        ,Helpers.Spells.CastSelf("Preparation", ret => Helpers.Spells.GetSpellCooldown("Vanish") > 30)
                    )
                ),

                // CP Builders
                new Decorator(ret => Helpers.Rogue.mComboPoints != 5 && (Helpers.Rogue.mComboPoints < 4 ||
                                     (Helpers.Rogue.mComboPoints == 4 && (Helpers.Rogue.mCurrentEnergy >= 90 ||
                                     Helpers.Aura.TimeRupture < 3 ||
                                     Helpers.Aura.ShadowDance))),
                    new PrioritySelector(
                        Helpers.Spells.Cast("Ambush", ret => (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)
                            && (Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) && Helpers.Aura.IsBehind),
                        Helpers.Spells.Cast("Fan of Knives", ret => Helpers.Rogue.IsAoeUsable() &&
                                                            Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 3),
                        Helpers.Spells.Cast("Hemorrhage", ret => (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)
                            && !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance)
                                                    && Helpers.Aura.TimeHemorrhage < 3),
                        Helpers.Spells.Cast("Backstab", ret => (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)
                            && !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) 
                                                   && Helpers.Rogue.mCurrentEnergy > 60 && Helpers.Aura.IsBehind),
                        Helpers.Spells.Cast("Hemorrhage", ret => (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)
                            && Helpers.Rogue.mCurrentEnergy > 70 && !Helpers.Aura.IsBehind
                          && !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) )
                    )
                ),

                Helpers.Spells.Cast("Redirect", ret => Helpers.Rogue.mComboPoints < Helpers.Rogue.me.RawComboPoints),
                new Decorator(ret => Helpers.Rogue.mTarget != null && Helpers.Spells.FindSpell(114014) && Helpers.Rogue.mCurrentEnergy > 20
                    && !Helpers.Aura.Stealth
                    && (Helpers.Rogue.mTarget.Distance > 10 && Helpers.Rogue.mTarget.Distance < 30
                    || !Helpers.Movement.IsInSafeMeleeRange && Helpers.Rogue.mTarget.Distance < 30 && Helpers.Rogue.mComboPoints < 5),
                    new Sequence(
                        new Action(ret =>
                        {
                            Styx.CommonBot.SpellManager.Cast("Throw", Helpers.Rogue.mTarget);
                            Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "Casting Shuriken Toss on target at " +
                            System.Math.Round(Helpers.Rogue.mTarget.HealthPercent, 0) + "% with " + Helpers.Rogue.mComboPoints + "CP and " +
                               Helpers.Rogue.mCurrentEnergy + " energy");
                        }),
                                new Action(ret => RunStatus.Failure)
                    )
                ),
                Helpers.Spells.CastFocus("Tricks of the Trade", ret => !Helpers.Aura.Tricks && Helpers.Focus.mFocusTarget != null
                                  && Helpers.Rogue.mCurrentEnergy > 40 && Helpers.Rogue.mCurrentEnergy < 75 && Helpers.Rogue.mComboPoints > 1)


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
