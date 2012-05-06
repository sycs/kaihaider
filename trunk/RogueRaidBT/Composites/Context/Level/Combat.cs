//////////////////////////////////////////////////
//               Level/Combat.cs                //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System;
using System.Linq;
using TreeSharp;
using Action = TreeSharp.Action;
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
                Helpers.Target.EnsureValidTarget(),
                Helpers.Movement.MoveToAndFaceUnit(ret => Helpers.Rogue.mTarget),

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

                new Decorator(ret => Helpers.Rogue.IsInterruptUsable(),
                    new Sequence(
                        Helpers.Spells.Cast("Kick"),
                        new WaitContinue(TimeSpan.FromSeconds(0.5), ret => false, new ActionAlwaysSucceed())
                    )
                ),

                Helpers.Spells.Cast("Kidney Shot",      ret => Helpers.Rogue.mComboPoints >= 3 && (Helpers.Rogue.mHP <= 75 || 
                                                               (Helpers.Rogue.IsInterruptUsable() && Helpers.Spells.GetSpellCooldown("Kick") > 0))),
                Helpers.Spells.Cast("Eviscerate",       ret => Helpers.Rogue.mComboPoints >= 4),
                Helpers.Spells.Cast("Revealing Strike", ret => Helpers.Rogue.mComboPoints == 4 &&
                                                               !Helpers.Aura.RevealingStrike),
                Helpers.Spells.Cast("Sinister Strike")
            );
        }

        public static Composite BuildPullBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret => StyxWoW.Me.Mounted, 
                    new Action(ret => Lua.DoString("Dismount()"))
                ),

                Helpers.Spells.CastSelf("Stealth", ret => !Helpers.Aura.Stealth),

                Helpers.Movement.MoveToAndFaceUnit(ret => Helpers.Rogue.mTarget),
                Helpers.Spells.Cast("Cheap Shot",  ret => Helpers.Aura.Stealth),
                Helpers.Spells.Cast("Sinister Strike")
            );
        }

        public static Composite BuildBuffBehavior()
        {
            return new Decorator(ret => !StyxWoW.Me.Mounted,
                new PrioritySelector(
                    Helpers.Spells.CastSelf("Recuperate", ret => !Helpers.Spells.IsAuraActive(StyxWoW.Me, "Recuperate") &&
                                                                     Helpers.Rogue.mRawComboPoints >= 1 && Helpers.Rogue.ResetRawComboPoints()),
                    Helpers.Spells.CastSelf("Slice and Dice", ret => !Helpers.Spells.IsAuraActive(StyxWoW.Me, "Slice and Dice") &&
                                                                     Helpers.Rogue.mRawComboPoints >= 1 && Helpers.Rogue.ResetRawComboPoints())
                )
            );
        }
    }
}
