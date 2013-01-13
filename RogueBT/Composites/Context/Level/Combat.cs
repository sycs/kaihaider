//////////////////////////////////////////////////
//               Level/Combat.cs                //
//      Part of RogueBT by kaihaider            //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////


using System.Linq;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;
using CommonBehaviors.Actions;
using Styx;
using Styx.WoWInternals;

namespace RogueBT.Composites.Context.Level
{
    class Combat
    {
        public static Composite BuildCombatBehavior()
        {
            return new PrioritySelector(
                Helpers.Movement.PleaseStop(),
                //Helpers.Target.EnsureValidTarget(),
                Helpers.Movement.MoveToLos(),
                Helpers.Movement.WalkBackwards(),
                Helpers.Movement.ChkFace(),
                Helpers.Spells.Cast("Shadowstep", ret => !Helpers.Movement.IsInSafeMeleeRange
                            && !Helpers.Aura.CripplingPoison && !Helpers.Aura.DeadlyThrow &&
                            Helpers.Rogue.mTarget.InLineOfSpellSight && Helpers.Rogue.mTarget.Distance < 25),
                Helpers.Spells.ToggleAutoAttack(),

                Helpers.Rogue.TryToInterrupt(ret => Helpers.Aura.IsTargetCasting != 0 && Helpers.Rogue.mTarget.CanInterruptCurrentSpellCast && 
                    Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds <= 0.6 && Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds >= 0.2),


                Helpers.Spells.CastSelf("Recuperate", ret => Helpers.Rogue.mComboPoints > 2 && Helpers.Rogue.mHP < 85 &&
                                Helpers.Aura.TimeRecuperate < 3), 

                new Decorator(ret => Helpers.Rogue.mHP <= 15 && Helpers.Spells.CanCast("Vanish"),
                    new Sequence(
                        Helpers.Spells.CastSelf("Vanish"),
                        new WaitContinue(2, ret => false, new ActionAlwaysSucceed())
                    )
                ),

                new Decorator(ret => Helpers.Rogue.mHP < 75,
                    new PrioritySelector(
                        Helpers.Spells.CastCooldown("Kidney Shot", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish) &&
                            Helpers.Rogue.mComboPoints > 3
                             && !Helpers.Rogue.mTarget.Silenced && !Helpers.Rogue.mTarget.Stunned
                             && !Helpers.Aura.IsTargetImmuneStun && Helpers.Movement.IsInSafeMeleeRange),
                        Helpers.Spells.CastSelf("Evasion", ret => Helpers.Rogue.mTarget != null && !Helpers.Rogue.mTarget.Stunned),
                        Helpers.Spells.Cast("Combat Readiness", ret => !Helpers.Rogue.me.HasAura("Evasion") && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 1),
                        Helpers.Spells.CastSelf("Cloak of Shadows", ret => !Helpers.Rogue.me.HasAura("Evasion") && !Helpers.Rogue.me.HasAura("Evasion") && Helpers.Rogue.IsCloakUsable()),
                        //Helpers.Spells.Cast("Dismantle", ret => !Helpers.Rogue.mTarget.Disarmed && Helpers.Rogue.CheckSpamLock()
                        //     && !Helpers.Rogue.mTarget.Silenced && !Helpers.Rogue.mTarget.Stunned
                        //     && !Helpers.Aura.IsTargetImmuneStun && Helpers.Movement.IsInSafeMeleeRange),
                        Helpers.Spells.Cast("Shiv", ret => Helpers.Aura.Leeching && Helpers.Rogue.mHP < 40)
                    )
                ),

                new Decorator(ret => Helpers.Spells.CanCast("Vanish") && Helpers.Rogue.mHP < 45 && !Helpers.Rogue.IsAoeUsable() &&
                                             Helpers.Aura.KidneyTime > 4,
                            new Sequence(
                                Helpers.Spells.CastSelf("Vanish"),
                                Helpers.Rogue.CreateWaitForLagDuration(),
                                Helpers.Movement.MoveToTarget(),
                                Helpers.Spells.Cast("Cheap Shot", ret => Helpers.Movement.IsInSafeMeleeRange)
                                )
                            ),

                new Decorator(ret => !Helpers.Rogue.IsAoeUsable() && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 1 &&
                                        Helpers.Rogue.mHP < 85 && Helpers.Target.GetCCTarget(),
                    new PrioritySelector(
                        Helpers.Spells.Cast("Blind", ret => Helpers.Rogue.mHP < 65 && Helpers.Target.BlindCCUnit != null, ret => Helpers.Target.BlindCCUnit),
                        
                        Helpers.Spells.Cast("Gouge", ret => Helpers.Target.GougeCCUnit!=null, ret => Helpers.Target.GougeCCUnit)
                    )
                ),

                 new Decorator(ret => !Helpers.Aura.IsTargetInvulnerable && !Helpers.Aura.IsTargetSapped && !Helpers.Aura.IsTargetDisoriented &&
                                        (Helpers.Rogue.mComboPoints > 3 || Helpers.Aura.FuryoftheDestroyer),
                    new PrioritySelector(
                         Helpers.Spells.CastSelf("Slice and Dice", ret => !Helpers.Aura.SliceandDice && Helpers.Rogue.mComboPoints == 5),

                        Helpers.Spells.Cast("Rupture", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                                    !Helpers.Aura.Rupture && Helpers.Movement.IsInSafeMeleeRange),

                        Helpers.Spells.Cast("Crimson Tempest", ret => Helpers.Rogue.IsAoeUsable() &&
                                                            Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 3),

                        Helpers.Spells.Cast("Eviscerate", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish) && Helpers.Movement.IsInSafeMeleeRange)

                    )

                ),

                Helpers.Specials.UseSpecialAbilities(),

                Helpers.Spells.CastSelf("Adrenaline Rush", ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Rogue.IsCooldownsUsable()),
                Helpers.Spells.CastSelf("Shadow Blades", ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Rogue.IsCooldownsUsable()),
                Helpers.Spells.CastCooldown("Killing Spree", ret => Helpers.Movement.IsInSafeMeleeRange &&  Helpers.Rogue.mTarget.Distance <10
                    && Helpers.Rogue.IsCooldownsUsable() && !Helpers.Aura.AdrenalineRush),


                Helpers.Spells.CastSelf("Blade Flurry", ret => Helpers.Rogue.IsAoeUsable() && !Helpers.Aura.BladeFlurry &&
                                                               Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.IsWithinMeleeRange) > 1
                                                               && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 15) < 3),

                new Decorator(ret => Helpers.Rogue.IsAoeUsable() && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 15) < 2 && 
                                     Helpers.Aura.BladeFlurry,
                    // Ugly. Find a way to cancel auras without Lua.
                    new Action(ret => Lua.DoString("RunMacroText('/cancelaura Blade Flurry');"))
                ),




                Helpers.Spells.Cast("Revealing Strike", ret => Helpers.Movement.IsInSafeMeleeRange && !Helpers.Aura.RevealingStrike && Helpers.Rogue.ReleaseSpamLock()),

                Helpers.Spells.Cast("Fan of Knives", ret => Helpers.Rogue.IsAoeUsable() && Helpers.Rogue.ReleaseSpamLock() &&
                                                            Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 1),

                Helpers.Spells.Cast("Sinister Strike", ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Rogue.ReleaseSpamLock()),

               
                Helpers.Movement.MoveToTarget(),

                
                
                Helpers.Spells.Cast("Redirect", ret => Helpers.Rogue.mComboPoints < Helpers.Rogue.me.RawComboPoints)
            );
        }

        static public Composite BuildPullBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret =>  Helpers.Rogue.me.Mounted, 
                    new Action(ret => Lua.DoString("Dismount()"))
                ),
                Helpers.Movement.PleaseStopPull(),
                //Helpers.Target.EnsureValidTarget(),
                Helpers.Movement.ChkFace(),
                Helpers.Movement.MoveToLos(),
                Helpers.Spells.Cast("Throw", ret => Helpers.Rogue.mTarget.IsFlying && Helpers.Rogue.mTarget.Distance > 5 && Helpers.Rogue.mTarget.Distance < 30),

                new Decorator(ret => !Helpers.Aura.Stealth && !Helpers.Aura.FaerieFire
                    && Helpers.Rogue.me.IsAlive && !Helpers.Rogue.me.Combat,
                    new Sequence(
                        Helpers.Spells.CastSelf("Stealth", ret => true),
                        Helpers.Rogue.CreateWaitForLagDuration()
                    )
                ),

                Helpers.Spells.Cast("Shadow Walk", ret => Helpers.Aura.Stealth && Helpers.Rogue.mTarget.Distance < 25),
                Helpers.Spells.Cast("Shadowstep", ret => !Helpers.Movement.IsInSafeMeleeRange &&
                            Helpers.Rogue.mTarget.InLineOfSpellSight && Helpers.Rogue.mTarget.Distance < 25),
                Helpers.Spells.Cast("Sap", ret => Helpers.Target.IsSappable()),


                /**
                new Decorator(ret => Helpers.Aura.Stealth && Helpers.Movement.IsInSafeMeleeRange 
                    && !Helpers.Rogue.mTarget.IsFlying,
                    new Sequence(
                        new Action(ret =>
                        {
                            Styx.CommonBot.SpellManager.Cast("Pick Pocket", Helpers.Rogue.mTarget);
                            Styx.Common.Logging.Write(Styx.Common.LogLevel.Diagnostic, "Pick Pocket attempted");
                            return RunStatus.Success;
                        }),
                        new Action(ret =>
                        {
                            Styx.CommonBot.SpellManager.Cast("Cheap Shot", Helpers.Rogue.mTarget);
                            Styx.Common.Logging.Write(Styx.Common.LogLevel.Diagnostic, "Cheap Shot attempted");
                            return RunStatus.Failure;
                        }),
                        Helpers.Rogue.CreateWaitForLagDuration()
                       
                    )
                ), **/

                new Decorator(ret => Helpers.Aura.Stealth && Helpers.Movement.IsInSafeMeleeRange
                    && !Helpers.Rogue.mTarget.IsFlying,
                    new Sequence(
                        new Action(ret =>
                        {
                            Styx.CommonBot.SpellManager.Cast("Pick Pocket", Helpers.Rogue.mTarget);
                            Styx.Common.Logging.Write(Styx.Common.LogLevel.Diagnostic, "Pick Pocket attempted");
                            return RunStatus.Failure;
                        })
                    )
                ),
                Helpers.Spells.Cast("Cheap Shot", ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Aura.Stealth),
                Helpers.Spells.Cast("Sinister Strike", ret => Helpers.Movement.IsInSafeMeleeRange),
                Helpers.Spells.Cast("Fan of Knives", ret => (Helpers.Rogue.mTarget == null || Helpers.Rogue.mTarget.IsFriendly)
                    && Helpers.Rogue.IsAoeUsable() && !Helpers.Rogue.me.HasAura("Stealth")),
                Helpers.Movement.PullMoveToTarget()


            );
        }

        public static Composite BuildBuffBehavior()
        {
            return new Decorator(ret => !Helpers.Rogue.me.Mounted,
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
