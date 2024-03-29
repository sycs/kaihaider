﻿//////////////////////////////////////////////////
//       Battleground/Assassination.cs          //
//      Part of MutaRaidBT by fiftypence        //
//////////////////////////////////////////////////

using System;
using Styx;
using System.Linq;
using CommonBehaviors.Actions;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;
using Styx.WoWInternals;

namespace RogueBT.Composites.Context.Battleground
{
    static class Assassination
    {
        // For now, just use the same behavior as our level context.

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
                                Helpers.Spells.Cast("Mutilate", ret => !Helpers.Aura.IsBehind)
                        )
                ),
                Helpers.Spells.Cast("Shadowstep", ret => !Helpers.Aura.IsTargetInvulnerable && Helpers.Rogue.mTarget.Distance > 15
                            && !Helpers.Aura.CripplingPoison && !Helpers.Aura.DeadlyThrow &&
                            Helpers.Rogue.mTarget.InLineOfSpellSight && Helpers.Rogue.mTarget.Distance < 25),
                Helpers.Rogue.TryToInterrupt(ret => Helpers.Aura.IsTargetCasting != 0 && Helpers.Rogue.mTarget.CanInterruptCurrentSpellCast &&
                    Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds <= 0.6 && Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds >= 0.2),

                Helpers.Spells.Cast("Dispatch", ret => Helpers.Aura.Blindside && (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)),
                Helpers.Spells.Cast("Envenom", ret => Helpers.Aura.DeadlyPoison && (Helpers.Aura.TimeSliceandDice <= 3 && Helpers.Aura.SliceandDice && Helpers.Rogue.mComboPoints > 0)
                                    && (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)),
                Helpers.Spells.CastCooldown("Feint", ret => !Helpers.Aura.Feint && !Helpers.Aura.Stealth && Settings.Mode.mFeint),
                Helpers.Spells.CastSelf("Recuperate", ret => Helpers.Rogue.mComboPoints > 2 && Helpers.Rogue.mHP < 95 && Helpers.Aura.TimeRecuperate < 3),
                new Decorator(ret => Settings.Mode.mVanish && Helpers.Rogue.mHP <= 10 && Helpers.Spells.CanCast("Vanish"),
                    new Sequence(
                        Helpers.Spells.CastSelf("Vanish"),
                        new WaitContinue(2, ret => false, new ActionAlwaysSucceed())
                    )
                ),

                Helpers.Target.BlindAdd(),
                Helpers.Target.GougeAdd(),

                new Decorator(ret => Helpers.Rogue.mHP < 75,
                    new PrioritySelector(
                        Helpers.Spells.CastCooldown("Kidney Shot", ret => !(Helpers.Aura.IsTargetInvulnerable || Helpers.Aura.Stealth || Helpers.Aura.Vanish) &&
                            Helpers.Rogue.mComboPoints > 3
                             && !Helpers.Rogue.mTarget.Silenced && !Helpers.Rogue.mTarget.Stunned
                             && !Helpers.Aura.IsTargetImmuneStun && Helpers.Movement.IsInSafeMeleeRange),
                        Helpers.Spells.Cast("Shiv", ret => !Helpers.Aura.IsTargetInvulnerable && Helpers.Movement.IsInSafeMeleeRange && Helpers.Aura.Leeching),
                        Helpers.Spells.CastSelf("Evasion", ret => Helpers.Rogue.mTarget != null && !Helpers.Rogue.mTarget.Stunned && !Helpers.Rogue.me.HasAura("Combat Readiness")),
                        Helpers.Spells.Cast("Combat Readiness", ret => !Helpers.Rogue.me.HasAura("Evasion")),
                        Helpers.Spells.CastSelf("Cloak of Shadows", ret => !Helpers.Rogue.me.HasAura("Evasion") && Helpers.Rogue.IsCloakUsable()),
                        Helpers.Spells.Cast("Smoke Bomb", ret => Helpers.Movement.IsInSafeMeleeRange
                            && Helpers.Target.mNearbyEnemyUnits.Count(unit => !(unit.Guid == Helpers.Target.BlindCCUnitGUID && unit.HasAura("Blind")
                                                    || unit.Guid == Helpers.Target.SapCCUnitGUID && unit.HasAura("Sap")) && unit.Distance > 10) > 0),
                        Helpers.Spells.Cast("Preparation", ret => Helpers.Rogue.mHP < 50 && !Helpers.Rogue.me.HasAura("Evasion")
                            && !Helpers.Rogue.me.HasAura("Combat Readiness") && !Helpers.Rogue.me.HasAura("Combat Insight")
                    && Helpers.Spells.GetSpellCooldown("Evasion") > 60 && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance > 10) > 2)

                    )
                ),
                Helpers.Spells.CastSelf("Slice and Dice", ret =>  Helpers.Rogue.mComboPoints > 0 && Helpers.Aura.TimeSliceandDice < 3),
                new Decorator(ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance)
                                        && (Helpers.Movement.IsInAttemptMeleeRange || !Settings.Mode.mUseMovement)
                                        && !Helpers.Aura.IsTargetInvulnerable && !Helpers.Aura.IsTargetSapped && !Helpers.Aura.IsTargetDisoriented
                                        && (Helpers.Rogue.mComboPoints == 5 || Helpers.Aura.FuryoftheDestroyer),
                    new PrioritySelector(
                        Helpers.Spells.Cast("Rupture", ret =>  !Helpers.Aura.Rupture),
                        Helpers.Spells.Cast("Envenom", ret => Helpers.Aura.DeadlyPoison && Helpers.Rogue.mComboPoints > 0
                                                       && (Helpers.Rogue.mComboPoints == 4 && Helpers.Rogue.mTargetHP >= 35 && !Helpers.Aura.Blindside
                                                       || Helpers.Rogue.mComboPoints > 4 && Helpers.Rogue.mTargetHP >= 35
                                                       || (Helpers.Rogue.mCurrentEnergy >= 55 && Helpers.Rogue.mComboPoints == 5 && Helpers.Rogue.mTargetHP < 35))
                                                        && (!Helpers.Aura.Envenom || Helpers.Rogue.mCurrentEnergy > 90))
                    )
                ),

                Helpers.Specials.UseSpecialAbilities(),
                Helpers.Spells.CastCooldown("Vendetta", ret => !Helpers.Aura.IsTargetInvulnerable && Helpers.Movement.IsInSafeMeleeRange && Helpers.Rogue.IsCooldownsUsable()),
                Helpers.Spells.CastSelf("Shadow Blades", ret => !Helpers.Aura.IsTargetInvulnerable && Helpers.Movement.IsInSafeMeleeRange && Helpers.Rogue.IsCooldownsUsable()),
                Helpers.Spells.Cast("Dispatch", ret => !Helpers.Aura.IsTargetInvulnerable && (Helpers.Rogue.mTargetHP < 35 || Helpers.Aura.Blindside)
                                                        && (!Helpers.Aura.Rupture || Helpers.Rogue.mCurrentEnergy >= 80 || Helpers.Aura.Envenom || Helpers.Aura.Blindside)
                                                       && (Helpers.Movement.IsInAttemptMeleeRange || !Settings.Mode.mUseMovement)),
                Helpers.Spells.Cast("Fan of Knives", ret => Helpers.Rogue.IsAoeUsable() && Helpers.Rogue.ReleaseSpamLock() && Helpers.Target.aoeSafe
                                        && (Helpers.Aura.Envenom || Helpers.Rogue.mCurrentEnergy > 80)
                                                            && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 1),
                Helpers.Spells.Cast("Mutilate", ret => !Helpers.Aura.IsTargetInvulnerable && Helpers.Movement.IsInAttemptMeleeRange && Helpers.Rogue.ReleaseSpamLock()),
                Helpers.Spells.CastSelf("Burst of Speed", ret =>Styx.CommonBot.SpellManager.HasSpell("Burst of Speed") && !Helpers.Aura.Stealth
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
                new Decorator(ret => Helpers.Rogue.me.Mounted,
                    new Action(ret => Lua.DoString("Dismount()"))
                ),
                new Decorator(ret => Helpers.Rogue.mTarget == null, new CommonBehaviors.Actions.ActionAlwaysSucceed()),
                Helpers.Movement.PleaseStopPull(),
                //Helpers.Target.SapAdd(),
                //Helpers.Target.EnsureValidTarget(),
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

                //Helpers.Spells.Cast("Cheap Shot", ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Aura.Stealth),
                //Helpers.Spells.CastFail("Cheap Shot", ret => Helpers.Movement.IsInAttemptMeleeRange && Helpers.Aura.Stealth, ret => Helpers.Rogue.mTarget),

                Helpers.Spells.CastFail("Mutilate", ret => Helpers.Movement.IsInAttemptMeleeRange && !Helpers.Aura.Stealth, ret => Helpers.Rogue.mTarget),
                Helpers.Spells.CastSelf("Burst of Speed", ret => Styx.CommonBot.SpellManager.HasSpell("Burst of Speed")
                     && !Helpers.Aura.Stealth && Helpers.Rogue.mCurrentEnergy > 90 && Helpers.Aura.ShouldBurst),
                Helpers.Spells.CastSelf("Sprint", ret => Helpers.Aura.Stealth && Helpers.Rogue.mTarget.Distance > 10),
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
                new Action(ret => RunStatus.Success)

            );
        }

        static public Composite BuildBuffBehavior()
        {
            return new Decorator(ret => !Helpers.Rogue.me.Mounted && !Helpers.Rogue.me.InVehicle,
                new PrioritySelector(
                    Helpers.Spells.CastSelf("Recuperate", ret => !Helpers.Spells.IsAuraActive(Helpers.Rogue.me, "Recuperate") &&
                                                                     Helpers.Rogue.mRawComboPoints >= 1 && Helpers.Rogue.CheckSpamLock()),
                    Helpers.Spells.CastSelf("Slice and Dice", ret => !Helpers.Spells.IsAuraActive(Helpers.Rogue.me, "Slice and Dice") &&
                                                                     Helpers.Rogue.mRawComboPoints >= 1 && Helpers.Rogue.CheckSpamLock()),
                    Helpers.Spells.CastSelf("Burst of Speed", ret => !Helpers.Aura.Stealth && Helpers.Rogue.mCurrentEnergy > 90
                    && Helpers.Rogue.me.IsMoving),
                    new Decorator(ret => Settings.Mode.mAlwaysStealth && !Helpers.Aura.Stealth && !Helpers.Aura.FaerieFire
                        && Helpers.Rogue.me.IsAlive && !Helpers.Rogue.me.Combat,
                        new Sequence(
                            Helpers.Spells.CastSelf("Stealth", ret => true),
                            Helpers.Rogue.CreateWaitForLagDuration()
                            )
                        )
                )
            );
        }
    }
}
