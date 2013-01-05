//////////////////////////////////////////////////
//           Level/Assassination.cs             //
//      Part of RogueRaidBT by kaihaider        //
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


namespace RogueRaidBT.Composites.Context.Level
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

                Helpers.Rogue.TryToInterrupt(ret => Helpers.Aura.IsTargetCasting != 0 && !Helpers.Aura.IsTargetInvulnerable &&

                    ((
                    Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds <= 0.6 &&
                    Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds >= 0.1) ||

                    (Helpers.Aura.IsTargetCasting == 740 || Helpers.Aura.IsTargetCasting == 47540 ||
                    Helpers.Aura.IsTargetCasting == 64843 || Helpers.Aura.IsTargetCasting == 12051 ||
                    Helpers.Aura.IsTargetCasting == 118 || Helpers.Aura.IsTargetCasting == 5782
                    ))),


                new Decorator(ret => Helpers.Rogue.mHP <= 15 && Helpers.Spells.CanCast("Vanish"),
                    new Sequence(
                        Helpers.Spells.CastSelf("Vanish"),
                        new WaitContinue(2, ret => false, new ActionAlwaysSucceed())
                    )
                ),

                new Decorator(ret => Helpers.Rogue.mHP < 75,
                    new PrioritySelector(

                        Helpers.Spells.CastCooldown("Kidney Shot", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish) &&

                            !Helpers.Rogue.mTarget.Silenced && !Helpers.Rogue.mTarget.Stunned &&
                            !Helpers.Aura.IsTargetImmuneStun && Helpers.Movement.IsInSafeMeleeRange),

                        Helpers.Spells.CastSelf("Evasion", ret => !Helpers.Rogue.mTarget.Stunned),

                        Helpers.Spells.Cast("Combat Readiness", ret => Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 1),
                        
                        Helpers.Spells.CastSelf("Cloak of Shadows", ret => Helpers.Rogue.IsCloakUsable())



                    )

                ),

                Helpers.Spells.CastSelf("Recuperate", ret => Helpers.Rogue.mComboPoints > 2 && Helpers.Rogue.mHP < 95 &&
                                Helpers.Aura.TimeRecuperate < 3), // Helpers.Spells.GetAuraTimeLeft(StyxWoW.Me, "Recuperate") 

                Helpers.Spells.CastSelf("Slice and Dice", ret => !Helpers.Aura.SliceandDice && Helpers.Rogue.mComboPoints == 5 &&
                    Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 2 ||
                    Helpers.Rogue.mComboPoints == 2 && Helpers.Aura.TimeSliceandDice < 3),
                
                Helpers.Spells.Cast("Envenom", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) && Helpers.Aura.SliceandDice &&
                            Helpers.Aura.TimeSliceandDice < 3 && Helpers.Movement.IsInSafeMeleeRange),

                new Decorator(ret => !Helpers.Aura.IsTargetInvulnerable && !Helpers.Aura.IsTargetSapped && !Helpers.Aura.IsTargetDisoriented &&
                                        (Helpers.Rogue.mComboPoints == 5 || Helpers.Aura.FuryoftheDestroyer),
                    new PrioritySelector(


                        Helpers.Spells.Cast("Rupture", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                                    !Helpers.Aura.Rupture && Helpers.Movement.IsInSafeMeleeRange),

                        Helpers.Spells.Cast("Crimson Tempest", ret => Helpers.Rogue.IsAoeUsable() &&
                                                            Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 2),

                        Helpers.Spells.Cast("Envenom", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance)),


                        Helpers.Spells.Cast("Eviscerate", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish) && !Helpers.Aura.IsTargetSapped &&
                                (Helpers.Aura.FindWeakness || Helpers.Aura.FuryoftheDestroyer || Helpers.Aura.ShadowDance) && Helpers.Movement.IsInSafeMeleeRange),


                        Helpers.Spells.Cast("Eviscerate", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish) && Helpers.Movement.IsInSafeMeleeRange)

                    )

                ), //Dispatch if target below 35% or Blindside aura on rouge

                Helpers.Specials.UseSpecialAbilities(),

                
                Helpers.Spells.CastCooldown("Vendetta",     ret => Helpers.Rogue.IsCooldownsUsable()),



                Helpers.Spells.Cast("Fan of Knives", ret => Helpers.Rogue.IsAoeUsable() && Helpers.Rogue.ReleaseSpamLock() &&
                                                            Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 6),


                Helpers.Spells.Cast("Dispatch", ret => Helpers.Rogue.ReleaseSpamLock() && Helpers.Rogue.mTargetHP < 35 || StyxWoW.Me.HasAura("Blindside")),

                Helpers.Spells.Cast("Mutilate", ret => Helpers.Rogue.ReleaseSpamLock()),


                Helpers.Movement.MoveToTarget(),

                Helpers.Spells.Cast("Redirect", ret => Helpers.Rogue.mComboPoints < StyxWoW.Me.RawComboPoints)
            );
        }

        static public Composite BuildPullBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret => StyxWoW.Me.Mounted, 
                    new Action(ret => Lua.DoString("Dismount()"))
                ),
                Helpers.Spells.CastSelf("Stealth", ret => !StyxWoW.Me.HasAura("Stealth") &&
                    StyxWoW.Me.IsAlive && !Helpers.Aura.FaerieFire && !StyxWoW.Me.IsAutoRepeatingSpell &&
                    !StyxWoW.Me.Combat),


                Helpers.Movement.MoveToTarget(),

                Helpers.Spells.Cast("Cheap Shot",  ret => StyxWoW.Me.HasAura("Stealth")),
                Helpers.Spells.Cast("Mutilate")
            );
        }

        static public Composite BuildBuffBehavior()
        {
            return new Decorator(ret => !StyxWoW.Me.Mounted,
                new PrioritySelector(
                    Helpers.Spells.CastSelf("Recuperate", ret => !Helpers.Spells.IsAuraActive(StyxWoW.Me, "Recuperate") &&
                                                                     Helpers.Rogue.mRawComboPoints >= 1 && Helpers.Rogue.CheckSpamLock()),
                    Helpers.Spells.CastSelf("Slice and Dice", ret => !Helpers.Spells.IsAuraActive(StyxWoW.Me, "Slice and Dice") &&
                                                                     Helpers.Rogue.mRawComboPoints >= 1 && Helpers.Rogue.CheckSpamLock())
                )
            );
        }
    }
}
