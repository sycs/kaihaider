//////////////////////////////////////////////////
//              Raid/Subtlety.cs                //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////


using System;
using Styx;
using System.Linq;
using CommonBehaviors.Actions;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;
using Styx.WoWInternals;

namespace RogueBT.Composites.Context.Level
{
    static class Subtlety
    {
        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(
                Helpers.Movement.PleaseStop(),
                //Helpers.Target.EnsureValidTarget(),
                Helpers.Movement.MoveToLos(),
                Helpers.Spells.Cast("Shadowstep", ret => !Helpers.Movement.IsInSafeMeleeRange
                            && !Helpers.Aura.CripplingPoison && !Helpers.Aura.DeadlyThrow &&
                            Helpers.Rogue.mTarget.InLineOfSpellSight && Helpers.Rogue.mTarget.Distance < 25),
                Helpers.Movement.ChkFace(),
                Helpers.Movement.WalkBackwards(),
                Helpers.Spells.ToggleAutoAttack(),

                Helpers.Spells.CastCooldown("Feint", ret => !Helpers.Aura.Feint &&
                    Helpers.Rogue.mTarget.IsWithinMeleeRange),

                    //kick on  tranquility, penance(needs testing), divine hymn, evocation, polymorph, fear
               Helpers.Rogue.TryToInterrupt(ret => Helpers.Aura.IsTargetCasting != 0 && Helpers.Rogue.mTarget.CanInterruptCurrentSpellCast &&
                    Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds <= 0.6 && Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds >= 0.2),

                //Helpers.Rogue.TryToInterrupt(ret => Helpers.Rogue.mTarget.CanInterruptCurrentSpellCast && !Helpers.Aura.IsTargetInvulnerable &&
                //    ((
                //    Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds <= 0.6 &&
                //    Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds >= 0.1) ||
                //    (Helpers.Aura.IsTargetCasting == 740 || Helpers.Aura.IsTargetCasting == 47540 ||
                //    Helpers.Aura.IsTargetCasting == 64843 || Helpers.Aura.IsTargetCasting == 12051 ||
                //    Helpers.Aura.IsTargetCasting == 118 || Helpers.Aura.IsTargetCasting == 5782
                //    ))),


                Helpers.Spells.CastSelf("Recuperate", ret => Helpers.Rogue.mComboPoints > 2 && Helpers.Rogue.mHP < 80 &&
                                Helpers.Aura.TimeRecuperate < 3),


                new Decorator(ret => Helpers.Rogue.mHP < 75,
                    new PrioritySelector(
                        Helpers.Spells.CastCooldown("Kidney Shot", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish) &&
                            Helpers.Rogue.mComboPoints > 3 &&
                            !Helpers.Rogue.mTarget.Silenced && !Helpers.Rogue.mTarget.Stunned &&
                            !Helpers.Aura.IsTargetImmuneStun && Helpers.Movement.IsInSafeMeleeRange),
                        Helpers.Spells.CastSelf("Evasion", ret => Helpers.Rogue.mTarget != null && !Helpers.Rogue.mTarget.Stunned),
                        Helpers.Spells.Cast("Combat Readiness", ret => Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 1),
                        Helpers.Spells.CastSelf("Cloak of Shadows", ret => Helpers.Rogue.IsCloakUsable()),
                        Helpers.Spells.Cast("Shiv", ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Aura.Leeching),
                        Helpers.Spells.Cast("Smoke Bomb", ret => Helpers.Movement.IsInSafeMeleeRange
                            && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance > 10) > 0)
                    )
                ),


                new Decorator(ret => !Helpers.Rogue.IsAoeUsable() && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 1 &&
                                        Helpers.Rogue.mHP < 85 && Helpers.Target.GetCCTarget(),
                    new PrioritySelector(
                        Helpers.Spells.Cast("Blind", ret => Helpers.Rogue.mHP < 65 && Helpers.Target.BlindCCUnit != null, ret => Helpers.Target.BlindCCUnit),

                        Helpers.Spells.Cast("Gouge", ret => Helpers.Target.GougeCCUnit != null, ret => Helpers.Target.GougeCCUnit)
                    )
                ),

                new Decorator(ret => !Helpers.Aura.IsTargetInvulnerable && !Helpers.Aura.IsTargetSapped && !Helpers.Aura.IsTargetDisoriented &&
                                        (Helpers.Rogue.mComboPoints == 5 || Helpers.Aura.FuryoftheDestroyer),
                    new PrioritySelector(
                        Helpers.Spells.CastSelf("Slice and Dice", ret => Helpers.Aura.TimeSliceandDice < 3),
                        Helpers.Spells.Cast("Crimson Tempest", ret => Helpers.Rogue.IsAoeUsable() &&
                                                            Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 2),
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

                        new Decorator(ret => !Helpers.Aura.ShadowDance &&
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

                        Helpers.Spells.CastSelf("Preparation", ret => Helpers.Spells.GetSpellCooldown("Vanish") > 30),

                        Helpers.Spells.CastSelf("Shadow Blades", ret => true)
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
                        Helpers.Spells.Cast("Fan of Knives", ret => Helpers.Rogue.IsAoeUsable() &&
                                                            Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 6),
                        Helpers.Spells.Cast("Hemorrhage", ret => Helpers.Movement.IsInSafeMeleeRange && !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance)
                                                    && Helpers.Aura.TimeHemorrhage < 3),
                        Helpers.Spells.Cast("Backstab", ret => Helpers.Movement.IsInSafeMeleeRange && !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                                                    Helpers.Rogue.mCurrentEnergy > 60 && Helpers.Aura.IsBehind),
                        Helpers.Spells.Cast("Hemorrhage", ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Rogue.mCurrentEnergy > 70 &&
                        !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance)
                                                                 && !Helpers.Aura.IsBehind)
                    )
                ),


                Helpers.Movement.MoveToTarget(),

                Helpers.Spells.CastFocus("Tricks of the Trade", ret => !Helpers.Aura.Tricks &&
                                                                       Helpers.Rogue.mCurrentEnergy < 60)

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
                //Helpers.Movement.MoveToLos(),
                Helpers.Spells.Cast("Throw", ret => Helpers.Movement.IsAboveTheGround(Helpers.Rogue.mTarget)
                    && System.Math.Abs(Helpers.Rogue.me.Z - Helpers.Rogue.mTarget.Z) >= 3 && Helpers.Rogue.mTarget.InLineOfSight
                    && Helpers.Rogue.mTarget.Distance > 5 && Helpers.Rogue.mTarget.Distance < 30),
                new Decorator(ret => !Helpers.Aura.Stealth && !Helpers.Aura.FaerieFire
                    && Helpers.Rogue.me.IsAlive && !Helpers.Rogue.me.Combat,
                    new Sequence(
                        Helpers.Spells.CastSelf("Stealth", ret => true),
                        Helpers.Rogue.CreateWaitForLagDuration()
                    )
                ),
                Helpers.Spells.Cast("Shadow Walk", ret => Helpers.Aura.Stealth && Helpers.Rogue.mTarget.Distance < 25),
                Helpers.Spells.Cast("Shadowstep", ret => Helpers.Rogue.mTarget.Distance > 10
                            && !Helpers.Aura.CripplingPoison && !Helpers.Aura.DeadlyThrow &&
                            Helpers.Rogue.mTarget.InLineOfSpellSight && Helpers.Rogue.mTarget.Distance < 25),
                //Helpers.Spells.Cast("Sap", ret => Helpers.Target.IsSappable()),

                new Decorator(ret => Helpers.Aura.Stealth && Helpers.Movement.IsInAttemptMeleeRange
                    && Styx.CommonBot.SpellManager.HasSpell("Pick Pocket"),
                    new Sequence(
                        new Action(ret =>
                        {
                            Styx.CommonBot.SpellManager.Cast("Pick Pocket", Helpers.Rogue.mTarget);
                            Styx.Common.Logging.Write(Styx.Common.LogLevel.Diagnostic, "Pick Pocket attempted");
                            return RunStatus.Failure;
                        })
                    )
                ),

                //Helpers.Spells.Cast("Ambush", ret => Helpers.Aura.IsBehind && Helpers.Aura.Stealth && Helpers.Movement.IsInSafeMeleeRange), //&& Helpers.Rogue.me.IsSafelyFacing(Helpers.Rogue.mTarget)
                new Decorator(ret => Helpers.Rogue.mTarget != null && Helpers.Movement.IsInAttemptMeleeRange 
                    && Helpers.Aura.Stealth && Helpers.Aura.IsBehind
                    && Styx.CommonBot.SpellManager.HasSpell("Ambush"),
                    new Sequence(
                        new Action(ret =>
                        {
                            Styx.CommonBot.SpellManager.Cast("Ambush", Helpers.Rogue.mTarget);
                            Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "Ambush attempted");
                            return RunStatus.Failure;
                        })
                    )
                ),

                //Helpers.Spells.Cast("Garrote", ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Aura.Stealth),
                new Decorator(ret => Helpers.Rogue.mTarget != null && Helpers.Movement.IsInAttemptMeleeRange
                    && Helpers.Aura.Stealth && !Helpers.Aura.IsBehind
                    && Styx.CommonBot.SpellManager.HasSpell("Garrote"),
                    new Sequence(
                        new Action(ret =>
                        {
                            Styx.CommonBot.SpellManager.Cast("Garrote", Helpers.Rogue.mTarget);
                            Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "Garrote attempted");
                            return RunStatus.Failure;
                        })
                    )
                ),

                //Helpers.Spells.Cast("Cheap Shot", ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Aura.Stealth),
                //new Decorator(ret => Helpers.Rogue.mTarget != null && Helpers.Movement.IsInAttemptMeleeRange && Helpers.Aura.Stealth
                //    && Styx.CommonBot.SpellManager.HasSpell("Cheap Shot"),
                //    new Sequence(
                //        new Action(ret =>
                //        {
                //            Styx.CommonBot.SpellManager.Cast("Cheap Shot", Helpers.Rogue.mTarget);
                //            Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "Cheap Shot attempted");
                //            return RunStatus.Failure;
                //        })
                //    )
                //),


                //Helpers.Spells.Cast("Hemorrhage", ret => Helpers.Movement.IsInSafeMeleeRange),
                new Decorator(ret => Helpers.Rogue.mTarget != null && Helpers.Movement.IsInAttemptMeleeRange && !Helpers.Aura.Stealth
                    && Styx.CommonBot.SpellManager.HasSpell("Sinister Strike"),
                    new Sequence(
                        new Action(ret =>
                        {
                            Styx.CommonBot.SpellManager.Cast("Sinister Strike", Helpers.Rogue.mTarget);
                            Styx.Common.Logging.Write(Styx.Common.LogLevel.Diagnostic, "Hemorrhage attempted");
                            return RunStatus.Failure;
                        })
                    )
                ),
                Helpers.Spells.Cast("Fan of Knives", ret => (Helpers.Rogue.mTarget == null || Helpers.Rogue.mTarget.IsFriendly)
                    && Helpers.Rogue.IsAoeUsable() && !Helpers.Aura.Stealth),
                Helpers.Movement.PullMoveToTarget(),
                new Decorator(ret => !Helpers.Rogue.mTarget.Stunned && Helpers.Movement.IsInSafeMeleeRange && Helpers.Rogue.mHP < 75
                            && Helpers.Spells.CanCast("Dismantle"),
                            new Sequence(
                                new Action(ret =>
                                {
                                    Styx.CommonBot.SpellManager.Cast("Dismantle", Helpers.Rogue.mTarget);
                                    Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "Dismantle attempted");
                                }),
                                new Action(ret => RunStatus.Failure)

                            )
                        )
            );
        }

        static public Composite BuildBuffBehavior()
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
