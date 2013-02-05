//////////////////////////////////////////////////
//              Raid/Subtlety.cs                //
//      Part of RogueBT by kaihaider        //
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
using Styx.WoWInternals;

namespace RogueBT.Composites.Context.Battleground
{
    static class Subtlety
    {
        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(
                Helpers.Movement.PleaseStop(),
                //Helpers.Target.EnsureValidTarget(),
                //Helpers.Movement.MoveToLos(),
                new Decorator(ret => Helpers.Rogue.mTarget == null, new CommonBehaviors.Actions.ActionAlwaysSucceed()),
                Helpers.Movement.MoveToTarget(),
                Helpers.Movement.ChkFace(),
                Helpers.Spells.ToggleAutoAttack(),
                new Decorator(ret => (Helpers.Aura.Stealth || Helpers.Aura.Vanish) && Helpers.Rogue.mTarget.IsWithinMeleeRange,
                    new PrioritySelector(
                                Helpers.Spells.Cast("Ambush", ret => Helpers.Aura.IsBehind),
                                Helpers.Spells.Cast("Garrote", ret => !Helpers.Aura.IsBehind && !Helpers.Rogue.mTarget.HasAura("Garrote"))
                        )
                ),
                Helpers.Spells.Cast("Shadowstep", ret => !Helpers.Aura.IsTargetInvulnerable && !Helpers.Movement.IsInSafeMeleeRange
                            && !Helpers.Aura.CripplingPoison && !Helpers.Aura.DeadlyThrow &&
                            Helpers.Rogue.mTarget.InLineOfSpellSight && Helpers.Rogue.mTarget.Distance < 25),
                Helpers.Spells.CastCooldown("Sap", ret => (Helpers.Aura.ShadowDance || Helpers.Aura.Stealth) && !Helpers.Aura.IsTargetSapped
                    && Helpers.Rogue.mTarget != null && !Helpers.Rogue.mTarget.Combat && Settings.Mode.mSap.Equals(Helpers.Enum.Saps.Target)
                     && Helpers.Rogue.mTarget.IsPlayer && Helpers.Rogue.mTarget.Distance < 10),


               Helpers.Rogue.TryToInterrupt(ret => Helpers.Aura.IsTargetCasting != 0 && Helpers.Rogue.mTarget.CanInterruptCurrentSpellCast &&
                    Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds <= 0.6 && Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds >= 0.2),
                Helpers.Spells.CastCooldown("Feint", ret => !Helpers.Aura.Feint && !Helpers.Aura.Stealth && Settings.Mode.mFeint),
                Helpers.Spells.CastSelf("Recuperate", ret => Helpers.Rogue.mComboPoints > 2 && Helpers.Rogue.mHP < 80 && Helpers.Aura.TimeRecuperate < 3),
                new Decorator(ret => Helpers.Rogue.mHP < 75,
                    new PrioritySelector(
                        Helpers.Spells.CastCooldown("Kidney Shot", ret => !(Helpers.Aura.IsTargetInvulnerable || Helpers.Aura.Stealth || Helpers.Aura.Vanish)
                             && Helpers.Rogue.mComboPoints > 3
                             && !Helpers.Rogue.mTarget.Silenced && !Helpers.Rogue.mTarget.Stunned
                             && !Helpers.Aura.IsTargetImmuneStun && Helpers.Movement.IsInSafeMeleeRange),
                        Helpers.Spells.Cast("Shiv", ret => !Helpers.Aura.IsTargetInvulnerable && Helpers.Movement.IsInSafeMeleeRange && Helpers.Aura.Leeching),
                        Helpers.Spells.CastSelf("Evasion", ret => Helpers.Rogue.mTarget != null && !Helpers.Rogue.mTarget.Stunned),
                        Helpers.Spells.Cast("Combat Readiness", ret => !Helpers.Rogue.me.HasAura("Evasion") && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 1),
                        Helpers.Spells.CastSelf("Cloak of Shadows", ret => !Helpers.Rogue.me.HasAura("Evasion") && Helpers.Rogue.IsCloakUsable()),
                        Helpers.Spells.Cast("Smoke Bomb", ret => Helpers.Movement.IsInSafeMeleeRange
                            && Helpers.Target.mNearbyEnemyUnits.Count(unit => !(unit.Guid == Helpers.Target.BlindCCUnitGUID && unit.HasAura("Blind")
                                                    || unit.Guid == Helpers.Target.SapCCUnitGUID && unit.HasAura("Sap")) && unit.Distance > 10) > 0),
                        Helpers.Spells.Cast("Preparation", ret => Helpers.Rogue.mHP < 50 && !Helpers.Rogue.me.HasAura("Evasion")
                            && !Helpers.Rogue.me.HasAura("Combat Readiness") && !Helpers.Rogue.me.HasAura("Combat Insight")
                            && Helpers.Spells.GetSpellCooldown("Evasion") > 60 && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance > 10) > 2)

                    )
                ),

                Helpers.Target.BlindAdd(),
                Helpers.Target.GougeAdd(),

                new Decorator(ret => !Helpers.Aura.IsTargetInvulnerable && !Helpers.Aura.IsTargetSapped && !Helpers.Aura.IsTargetDisoriented &&
                                        (Helpers.Rogue.mComboPoints == 5 || Helpers.Aura.FuryoftheDestroyer),
                    new PrioritySelector(
                        Helpers.Spells.CastSelf("Slice and Dice", ret => Helpers.Aura.TimeSliceandDice < 3),
                        Helpers.Spells.Cast("Crimson Tempest", ret => Helpers.Rogue.IsAoeUsable() &&
                                                            Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 3
                                       && (Helpers.Target.mNearbyEnemyUnits.Count(unit =>
                                           (unit.Guid == Helpers.Target.BlindCCUnitGUID && unit.HasAura("Blind")
                                                    || unit.Guid == Helpers.Target.SapCCUnitGUID && unit.HasAura("Sap"))
                        && unit.Distance < System.Math.Max(3.5f, Helpers.Rogue.me.CombatReach + 0.5333334f + unit.CombatReach)) == 0)),
                        Helpers.Spells.Cast("Rupture", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                                    !Helpers.Aura.FindWeakness && !Helpers.Aura.Rupture && Helpers.Movement.IsInSafeMeleeRange),
                        Helpers.Spells.Cast("Eviscerate", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish) && !Helpers.Aura.IsTargetSapped &&
                                (Helpers.Aura.FindWeakness || Helpers.Aura.FuryoftheDestroyer || Helpers.Aura.ShadowDance) && Helpers.Movement.IsInSafeMeleeRange),
                        Helpers.Spells.CastCooldown("Kidney Shot", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish) &&
                            !Helpers.Rogue.mTarget.Silenced && !Helpers.Rogue.mTarget.Stunned &&
                            !Helpers.Aura.IsTargetImmuneStun && Helpers.Movement.IsInSafeMeleeRange),
                        Helpers.Spells.Cast("Eviscerate", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish) && Helpers.Movement.IsInSafeMeleeRange)
                    )
                ),



                Helpers.Spells.CastCooldown("Premeditation", ret => Helpers.Rogue.mComboPoints <= 3 && !Helpers.Aura.IsTargetSapped && (Helpers.Aura.Stealth ||
                                                                    Helpers.Aura.ShadowDance || Helpers.Aura.Vanish)), // set range 30

                Helpers.Specials.UseSpecialAbilities(ret => Helpers.Aura.ShadowDance ||
                                                            Helpers.Spells.GetSpellCooldown("Shadow Dance") >= 10),

                new Decorator(ret => Helpers.Rogue.IsCooldownsUsable() &&
                    Helpers.Rogue.mTarget != null && Helpers.Movement.IsInSafeMeleeRange &&
                    !Helpers.Aura.IsTargetDisoriented && !Helpers.Aura.IsTargetInvulnerable &&
                                     Helpers.Rogue.mComboPoints == 0 && Helpers.Rogue.mHP > 85 &&
                                     Helpers.Rogue.mCurrentEnergy >= 50 &&
                                     !(Helpers.Spells.GetSpellCooldown("Premeditation") > 0),
                    new PrioritySelector(
                        new Decorator(ret => Helpers.Spells.CanCast("Shadow Dance"),
                            new Sequence(
                                Helpers.Spells.CastSelf("Shadow Dance"),
                                Helpers.Rogue.CreateWaitForLagDuration()
                            )
                        ),

                        new Decorator(ret => !Helpers.Aura.ShadowDance && !Helpers.Aura.IsTargetInvulnerable &&
                                             Helpers.Spells.GetSpellCooldown("Shadow Dance") > 0 &&
                                             Helpers.Spells.CanCast("Vanish") && !Helpers.Rogue.IsAoeUsable() &&
                                             Helpers.Aura.KidneyTime > 4,
                            new Sequence(
                                Helpers.Spells.CastSelf("Vanish", ret => true),
                                Helpers.Rogue.CreateWaitForLagDuration(),
                                Helpers.Spells.CastCooldown("Premeditation"),
                                Helpers.Spells.Cast("Garrote", ret => Helpers.Aura.IsBehind && Helpers.Movement.IsInSafeMeleeRange)
                            )
                        ),

                        Helpers.Spells.CastSelf("Preparation", ret => !Helpers.Aura.IsTargetInvulnerable && Helpers.Spells.GetSpellCooldown("Vanish") > 30),

                        Helpers.Spells.CastSelf("Shadow Blades", ret => !Helpers.Aura.IsTargetInvulnerable)
                    )
                ),


                // CP Builders
                new Decorator(ret => Helpers.Rogue.mTarget != null && !Helpers.Aura.IsTargetDisoriented &&
                                     !Helpers.Aura.IsTargetInvulnerable &&
                                     Helpers.Rogue.mComboPoints != 5 && (Helpers.Rogue.mComboPoints < 4 ||
                                     (Helpers.Rogue.mComboPoints == 4 && (Helpers.Rogue.mCurrentEnergy >= 90 ||
                                     Helpers.Aura.TimeRupture < 3 ||
                                     Helpers.Aura.ShadowDance))) && !Helpers.Aura.IsTargetSapped &&
                                     Helpers.Rogue.ReleaseSpamLock(),
                    new PrioritySelector(

                        Helpers.Spells.Cast("Garrote", ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Aura.IsTargetCasting != 0 &&
                            (Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                            Helpers.Focus.rawFocusTarget != null && Helpers.Focus.rawFocusTarget == Helpers.Rogue.mTarget &&
                                !Helpers.Rogue.mTarget.Silenced && !Helpers.Rogue.mTarget.Stunned && Helpers.Aura.IsBehind
                               ),

                        Helpers.Spells.Cast("Ambush", ret => Helpers.Movement.IsInSafeMeleeRange && (Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                            Helpers.Aura.IsBehind),
                        Helpers.Spells.CastCooldown("Cheap Shot", ret => Helpers.Movement.IsInSafeMeleeRange && (Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                                                             !Helpers.Rogue.mTarget.Stunned && !Helpers.Rogue.mTarget.Silenced),
                        Helpers.Spells.Cast("Fan of Knives", ret => Helpers.Rogue.IsAoeUsable() && Helpers.Rogue.ReleaseSpamLock() && Helpers.Target.aoeSafe
                                                            && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 1),
                        Helpers.Spells.Cast("Hemorrhage", ret => Helpers.Movement.IsInSafeMeleeRange && !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance)
                                                    && Helpers.Aura.TimeHemorrhage < 3),
                        Helpers.Spells.Cast("Backstab", ret => Helpers.Movement.IsInSafeMeleeRange && !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                                                    Helpers.Rogue.mCurrentEnergy > 60 && Helpers.Aura.IsBehind),
                        Helpers.Spells.Cast("Hemorrhage", ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Rogue.mCurrentEnergy > 85 &&
                        !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance)
                                                                 && !Helpers.Aura.IsBehind)
                    )
                ),

                Helpers.Spells.CastSelf("Burst of Speed", ret => Styx.CommonBot.SpellManager.HasSpell("Burst of Speed") && !Helpers.Aura.Stealth
                     && (Helpers.Rogue.mCurrentEnergy > 90 && Helpers.Rogue.mTarget.Distance > 8 && Helpers.Rogue.mTarget.IsMoving || Helpers.Aura.ShouldBurst && !Helpers.Movement.IsInAttemptMeleeRange)),
                Helpers.Spells.Cast("Redirect", ret => Helpers.Rogue.mComboPoints < Helpers.Rogue.me.RawComboPoints),
                new Decorator(ret => Helpers.Spells.FindSpell(114014) && Helpers.Rogue.mCurrentEnergy > 20
                    && !Helpers.Aura.Stealth && Helpers.Rogue.me.IsSafelyFacing(Helpers.Rogue.mTarget)
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
                                new CommonBehaviors.Actions.ActionAlwaysFail()
                    )
                ),

                Helpers.Spells.Cast("Dismantle", ret => !Helpers.Rogue.mTarget.Stunned && Helpers.Movement.IsInSafeMeleeRange && Helpers.Rogue.mHP < 75),
                new Action(ret => RunStatus.Success)

            );
        }

        static public Composite BuildPullBehavior()
        {
            return new PrioritySelector(
                Helpers.Movement.PleaseStopPull(),
                //Helpers.Target.EnsureValidTarget(),
                new Decorator(ret => Helpers.Rogue.mTarget == null, new CommonBehaviors.Actions.ActionAlwaysSucceed()),
                Helpers.Movement.MoveToTarget(),
                Helpers.Movement.ChkFace(),
                //Helpers.Movement.MoveToLos(),
                new Decorator(ret => !Helpers.Aura.Stealth && !Helpers.Aura.FaerieFire
                    && Helpers.Rogue.me.IsAlive && !Helpers.Rogue.me.Combat,
                    new Sequence(
                        Helpers.Spells.CastSelf("Stealth", ret => true),
                        Helpers.Rogue.CreateWaitForLagDuration()
                    )
                ),
                Helpers.Spells.Cast("Shadow Walk", ret => Helpers.Aura.Stealth && Helpers.Rogue.mTarget.Distance < 25 && Helpers.Rogue.mTarget.Distance > 10),
                Helpers.Spells.Cast("Shadowstep", ret => Helpers.Rogue.mTarget.Distance > 10
                            && !Helpers.Aura.CripplingPoison && !Helpers.Aura.DeadlyThrow &&
                            Helpers.Rogue.mTarget.InLineOfSpellSight && Helpers.Rogue.mTarget.Distance < 25),
               Helpers.Spells.Cast("Sap", ret => Settings.Mode.mSap.Equals(Helpers.Enum.Saps.Target) && Helpers.Target.IsSappable(Helpers.Rogue.mTarget)),
                Helpers.Spells.CastFail("Ambush", ret => Helpers.Movement.IsInAttemptMeleeRange && Helpers.Aura.IsBehind && Helpers.Aura.Stealth, ret => Helpers.Rogue.mTarget),
                Helpers.Spells.CastFail("Garrote", ret => Helpers.Movement.IsInAttemptMeleeRange && Helpers.Aura.Stealth, ret => Helpers.Rogue.mTarget),
                //Helpers.Spells.Cast("Cheap Shot", ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Rogue.me.HasAura("Stealth")),
                Helpers.Spells.CastFail("Sinister Strike", ret => Helpers.Movement.IsInAttemptMeleeRange && !Helpers.Aura.Stealth, ret => Helpers.Rogue.mTarget),

                Helpers.Spells.Cast("Fan of Knives", ret => (Helpers.Rogue.mTarget == null || Helpers.Rogue.mTarget.IsFriendly)
                    && Helpers.Rogue.IsAoeUsable() && !Helpers.Aura.Stealth),
                Helpers.Spells.CastSelf("Burst of Speed", ret => Styx.CommonBot.SpellManager.HasSpell("Burst of Speed")
                     && !Helpers.Aura.Stealth && Helpers.Rogue.mCurrentEnergy > 90 && Helpers.Aura.ShouldBurst),
                Helpers.Movement.PullMoveToTarget(),
                Helpers.Spells.CastSelf("Sprint", ret => Helpers.Aura.Stealth),
                new Action(ret => RunStatus.Success)
            );
        }

        static public Composite BuildBuffBehavior()
        {
            return new Decorator(ret => !Helpers.Rogue.me.Mounted,
                new PrioritySelector(
                    Helpers.Spells.CastSelf("Stealth", ret => !Helpers.Rogue.me.HasAura("Stealth") &&
                    Helpers.Rogue.me.IsAlive && !Helpers.Aura.FaerieFire && !Helpers.Rogue.me.IsAutoRepeatingSpell
                    &&
                !Helpers.Rogue.me.Combat),

                    Helpers.Spells.CastSelf("Recuperate", ret => !Helpers.Spells.IsAuraActive(Helpers.Rogue.me, "Recuperate") &&
                                                                     Helpers.Rogue.mRawComboPoints >= 1 && Helpers.Rogue.CheckSpamLock()),
                    Helpers.Spells.CastSelf("Slice and Dice", ret => !Helpers.Spells.IsAuraActive(Helpers.Rogue.me, "Slice and Dice") &&
                                                                     Helpers.Rogue.mRawComboPoints >= 1 && Helpers.Rogue.CheckSpamLock())
                )
            );
        }
    }
}
