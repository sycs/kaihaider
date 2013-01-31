//////////////////////////////////////////////////
//           Battleground/Combat.cs             //
//        Part of RogueBT by kaihaider          //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Linq;
using Styx.WoWInternals;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;
using CommonBehaviors.Actions;

namespace RogueBT.Composites.Context.Battleground
{
    class Combat
    {
        // For now, just use the same behavior as our level context.

        static public Composite BuildCombatBehavior()
        {

            return new PrioritySelector(
                Helpers.Movement.PleaseStop(),
                //Helpers.Target.EnsureValidTarget(),
                //Helpers.Movement.MoveToLos(),
                Helpers.Movement.MoveToTarget(),
                Helpers.Movement.ChkFace(),
                Helpers.Spells.Cast("Shadowstep", ret => !Helpers.Movement.IsInSafeMeleeRange
                            && !Helpers.Aura.CripplingPoison && !Helpers.Aura.DeadlyThrow &&
                            Helpers.Rogue.mTarget.InLineOfSpellSight && Helpers.Rogue.mTarget.Distance < 25),
                Helpers.Spells.ToggleAutoAttack(),
                Helpers.Rogue.TryToInterrupt(ret => Helpers.Aura.IsTargetCasting != 0 && Helpers.Rogue.mTarget.CanInterruptCurrentSpellCast &&
                    Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds <= 0.6 && Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds >= 0.2),
                Helpers.Spells.CastCooldown("Feint", ret => !Helpers.Aura.Feint && !Helpers.Aura.Stealth && Settings.Mode.mFeint),
                Helpers.Spells.CastSelf("Recuperate", ret => Helpers.Rogue.mComboPoints > 2 && Helpers.Rogue.mHP < 85 &&
                                Helpers.Aura.TimeRecuperate < 3),
                new Decorator(ret => Helpers.Spells.CanCast("Vanish") && Helpers.Rogue.mHP < 15,
                            new Sequence(
                                Helpers.Spells.CastSelf("Vanish"),
                                Helpers.Rogue.CreateWaitForLagDuration(),
                                Helpers.Movement.MoveToTarget(),
                                Helpers.Spells.Cast("Cheap Shot", ret => Helpers.Movement.IsInSafeMeleeRange)
                                )
                            ),
                new Decorator(ret => Helpers.Rogue.mHP < 75,
                    new PrioritySelector(
                        Helpers.Spells.CastCooldown("Kidney Shot", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish) &&
                            Helpers.Rogue.mComboPoints > 3
                             && !Helpers.Rogue.mTarget.Silenced && !Helpers.Rogue.mTarget.Stunned
                             && !Helpers.Aura.IsTargetImmuneStun && Helpers.Movement.IsInSafeMeleeRange),
                        Helpers.Spells.Cast("Shiv", ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Aura.Leeching),
                        Helpers.Spells.CastSelf("Evasion", ret => Helpers.Rogue.mTarget != null && !Helpers.Rogue.mTarget.Stunned),
                        Helpers.Spells.Cast("Combat Readiness", ret => !Helpers.Aura.Evasion && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 1),
                        Helpers.Spells.CastSelf("Cloak of Shadows", ret => !Helpers.Aura.Evasion && Helpers.Rogue.IsCloakUsable()),
                        Helpers.Spells.Cast("Smoke Bomb", ret => Helpers.Movement.IsInSafeMeleeRange
                            && Helpers.Target.mNearbyEnemyUnits.Count(unit => !(unit.Guid == Helpers.Target.BlindCCUnitGUID && unit.HasAura("Blind")
                                                    || unit.Guid == Helpers.Target.SapCCUnitGUID && unit.HasAura("Sap")) && unit.Distance > 10) > 0),
                        Helpers.Spells.Cast("Preparation", ret => Helpers.Rogue.mHP < 50 && !Helpers.Aura.Evasion
                            && !Helpers.Rogue.me.HasAura("Combat Readiness") && !Helpers.Rogue.me.HasAura("Combat Insight")
                    && Helpers.Spells.GetSpellCooldown("Evasion") > 60 && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance > 10) > 2)

                    )
                ),

                Helpers.Target.BlindAdd(),
                Helpers.Target.GougeAdd(),

                 new Decorator(ret => !Helpers.Aura.IsTargetInvulnerable && !Helpers.Aura.IsTargetSapped && !Helpers.Aura.IsTargetDisoriented &&
                                        (Helpers.Rogue.mComboPoints > 3 || Helpers.Aura.FuryoftheDestroyer),
                    new PrioritySelector(
                         Helpers.Spells.CastSelf("Slice and Dice", ret => !Helpers.Aura.SliceandDice && Helpers.Rogue.mComboPoints == 5),

                        Helpers.Spells.Cast("Rupture", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance)
                                    && !Helpers.Aura.Rupture && Helpers.Movement.IsInSafeMeleeRange),

                        Helpers.Spells.Cast("Eviscerate", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish) && Helpers.Movement.IsInSafeMeleeRange)

                    )

                ),



                Helpers.Specials.UseSpecialAbilities(),

                Helpers.Spells.CastSelf("Adrenaline Rush", ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Rogue.IsCooldownsUsable()),
                Helpers.Spells.CastSelf("Shadow Blades", ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Rogue.IsCooldownsUsable()),

                new Decorator(ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Rogue.IsCooldownsUsable() && !Helpers.Aura.AdrenalineRush
                    && Styx.CommonBot.SpellManager.HasSpell("Killing Spree")
                    && ((Helpers.Spells.GetSpellCooldown("Killing Spree") < 0.2) || (Styx.CommonBot.SpellManager.GlobalCooldown && Helpers.Spells.GetSpellCooldown("Killing Spree") < 0.5)),
                    new Sequence(
                        new Action(ret =>
                        {
                            Styx.CommonBot.SpellManager.Cast("Killing Spree", Helpers.Rogue.me);
                            Styx.Common.Logging.Write(Styx.Common.LogLevel.Diagnostic, "Killing Spree attempted");
                            return RunStatus.Failure;
                        })
                    )
                ),

                Helpers.Spells.CastSelf("Blade Flurry", ret => Helpers.Rogue.IsAoeUsable() && !Helpers.Aura.BladeFlurry &&
                                                               Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.IsWithinMeleeRange) > 1),

                new Decorator(ret => Helpers.Rogue.IsAoeUsable() && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 15) < 2 &&
                                     Helpers.Aura.BladeFlurry,
                // Ugly. Find a way to cancel auras without Lua.
                    new Action(ret => Lua.DoString("RunMacroText('/cancelaura Blade Flurry');"))
                ),
                Helpers.Spells.Cast("Revealing Strike", ret => Helpers.Movement.IsInSafeMeleeRange && !Helpers.Aura.RevealingStrike && Helpers.Rogue.ReleaseSpamLock()),
                Helpers.Spells.Cast("Fan of Knives", ret => Helpers.Rogue.IsAoeUsable() && Helpers.Rogue.ReleaseSpamLock() && Helpers.Target.aoeSafe
                                                            && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 2),
                Helpers.Spells.Cast("Sinister Strike", ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Rogue.ReleaseSpamLock()),
                Helpers.Spells.CastSelf("Burst of Speed", ret => Styx.CommonBot.SpellManager.HasSpell("Burst of Speed") && !Helpers.Aura.Stealth
                     && (Helpers.Rogue.mCurrentEnergy > 90 && Helpers.Rogue.mTarget.Distance > 8 || Helpers.Aura.ShouldBurst && !Helpers.Movement.IsInSafeMeleeRange)),
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
                Helpers.Movement.PleaseStopPull(),
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
                //Helpers.Spells.Cast("Cheap Shot", ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Aura.Stealth), //&& Helpers.Rogue.me.IsSafelyFacing(Helpers.Rogue.mTarget)
                Helpers.Spells.CastFail("Ambush", ret => Helpers.Movement.IsInAttemptMeleeRange && Helpers.Aura.IsBehind && Helpers.Aura.Stealth, ret => Helpers.Rogue.mTarget),
                Helpers.Spells.CastFail("Sinister Strike", ret => Helpers.Movement.IsInAttemptMeleeRange && !Helpers.Aura.Stealth, ret => Helpers.Rogue.mTarget),
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
            return new Decorator(ret => !Helpers.Rogue.me.Mounted,
                 new PrioritySelector(
                     Helpers.Spells.CastSelf("Recuperate", ret => !Helpers.Spells.IsAuraActive(Helpers.Rogue.me, "Recuperate") &&
                                                                      Helpers.Rogue.mRawComboPoints >= 1 && Helpers.Rogue.CheckSpamLock()),
                     Helpers.Spells.CastSelf("Slice and Dice", ret => !Helpers.Spells.IsAuraActive(Helpers.Rogue.me, "Slice and Dice") &&
                                         Helpers.Rogue.mRawComboPoints >= 1 && Helpers.Rogue.CheckSpamLock()), 

                 
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
