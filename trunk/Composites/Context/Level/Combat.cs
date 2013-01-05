//////////////////////////////////////////////////
//               Level/Combat.cs                //
//      Part of RogueRaidBT by kaihaider        //
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

namespace RogueRaidBT.Composites.Context.Level
{
    class Combat
    {
        public static Composite BuildCombatBehavior()
        {
            return new PrioritySelector(


                Helpers.Movement.MoveToTarget(),
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


                Helpers.Spells.ToggleAutoAttack(ret => !Helpers.Aura.Vanish && !Helpers.Aura.IsTargetDisoriented && !Helpers.Aura.IsTargetSapped),

                Helpers.Specials.UseSpecialAbilities(),

                Helpers.Spells.CastSelf("Evasion",           ret => Helpers.Rogue.mHP <= 35),
                Helpers.Spells.CastSelf("Cloak of Shadows",  ret => Helpers.Rogue.IsCloakUsable()),

                Helpers.Spells.Cast("Redirect",              ret => Helpers.Rogue.mComboPoints < StyxWoW.Me.RawComboPoints),

                Helpers.Spells.CastSelf("Adrenaline Rush",   ret => Helpers.Rogue.IsCooldownsUsable()),
                Helpers.Spells.CastCooldown("Killing Spree", ret => Helpers.Rogue.IsCooldownsUsable() && !Helpers.Aura.AdrenalineRush),

                Helpers.Spells.CastSelf("Blade Flurry", ret => Helpers.Rogue.IsAoeUsable() && !Helpers.Aura.BladeFlurry &&
                                                               Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.IsWithinMeleeRange) >= 2),

                new Decorator(ret => Helpers.Rogue.IsAoeUsable() && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 15) < 2 && 
                                     Helpers.Aura.BladeFlurry,
                    // Ugly. Find a way to cancel auras without Lua.
                    new Action(ret => Lua.DoString("RunMacroText('/cancelaura Blade Flurry');"))
                ),

                Helpers.Spells.CastSelf("Recuperate", ret => !Helpers.Aura.Recuperate && Helpers.Rogue.mComboPoints >= 3 && 
                                                             Helpers.Rogue.mHP <= 80),

                

                Helpers.Spells.Cast("Kidney Shot",      ret => Helpers.Rogue.mComboPoints >= 3 && (Helpers.Rogue.mHP <= 75 || 
                                                               (Helpers.Rogue.IsInterruptUsable() && Helpers.Spells.GetSpellCooldown("Kick") > 0))),
                Helpers.Spells.Cast("Eviscerate",       ret => Helpers.Rogue.mComboPoints >= 4),
                Helpers.Spells.Cast("Revealing Strike", ret => Helpers.Rogue.mComboPoints == 4 &&
                                                               !Helpers.Aura.RevealingStrike),
                Helpers.Spells.Cast("Sinister Strike", ret => Helpers.Rogue.ReleaseSpamLock())
            );
        }

        public static Composite BuildPullBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret => StyxWoW.Me.Mounted, 
                    new Action(ret => Lua.DoString("Dismount()"))
                ),
                Helpers.Spells.CastSelf("Stealth", ret => !StyxWoW.Me.HasAura("Stealth") &&
                    StyxWoW.Me.IsAlive && !Helpers.Aura.FaerieFire && !StyxWoW.Me.IsAutoRepeatingSpell &&
                    !StyxWoW.Me.Combat),

                Helpers.Movement.MoveToTarget(),

                Helpers.Spells.Cast("Cheap Shot",  ret => Helpers.Aura.Stealth),
                Helpers.Spells.Cast("Sinister Strike")
            );
        }

        public static Composite BuildBuffBehavior()
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
