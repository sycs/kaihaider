//////////////////////////////////////////////////
//               Raid/Combat.cs                 //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Linq;
using Styx;
using Styx.WoWInternals;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;

namespace RogueBT.Composites.Context.Arena
{
    class Combat
    {
        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(
                Helpers.Spells.ToggleAutoAttack(),

                Helpers.Spells.CastCooldown("Feint", ret => (Helpers.Aura.IsTargetCasting == 46924 || Helpers.Aura.IsTargetCasting == 1680) && 
                    Helpers.Rogue.mTarget.IsWithinMeleeRange),

                Helpers.Spells.CastCooldown("Sap", ret => (Helpers.Aura.ShadowDance || Helpers.Aura.Stealth) && !Helpers.Aura.IsTargetSapped &&
                    Helpers.Rogue.mTarget != Helpers.Focus.rawFocusTarget && !Helpers.Rogue.mTarget.Combat ),

                    //force kick on  tranquility, penance(needs testing), divine hymn, evocation, polymorph, fear
                    //Helpers.Rogue.mTarget.Class == Styx.Combat.CombatRoutine.WoWClass.Mage
                Helpers.Rogue.TryToInterrupt(ret => Helpers.Aura.IsTargetCasting != 0 && !Helpers.Aura.IsTargetInvulnerable &&

                    ((Helpers.Focus.rawFocusTarget != null && Helpers.Focus.rawFocusTarget == Helpers.Rogue.mTarget &&
                    Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds <= 0.6 &&
                    Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds >= 0.1 ) ||

                    (Helpers.Aura.IsTargetCasting == 740 || Helpers.Aura.IsTargetCasting == 47540 ||
                    Helpers.Aura.IsTargetCasting == 64843 || Helpers.Aura.IsTargetCasting == 12051 ||
                    Helpers.Aura.IsTargetCasting == 118 || Helpers.Aura.IsTargetCasting == 5782
                    ))),

                Helpers.Rogue.TryToInterruptFocus(ret => Helpers.Focus.rawFocusTarget != null && Helpers.Focus.rawFocusTarget != Helpers.Rogue.mTarget &&
                    !Helpers.Focus.rawFocusTarget.IsFriendly && Helpers.Focus.rawFocusTarget.IsWithinMeleeRange &&
                    Helpers.Focus.rawFocusTarget.IsCasting ),

                Helpers.Spells.CastSelf("Recuperate", ret => Helpers.Rogue.mComboPoints > 2 && Helpers.Rogue.mHP < 95 &&
                                Helpers.Aura.TimeRecuperate < 3),

                Helpers.Spells.Cast("Redirect", ret => Helpers.Rogue.me.ComboPoints < Helpers.Rogue.me.RawComboPoints),


                Helpers.Spells.CastSelf("Blade Flurry", ret => Helpers.Rogue.IsAoeUsable() && !Helpers.Rogue.me.HasAura("Blade Flurry") &&
                                                               Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.IsWithinMeleeRange) >= 2),

                new Decorator(ret => Helpers.Rogue.IsAoeUsable() && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 15) < 2 && 
                                     Helpers.Rogue.me.HasAura("Blade Flurry"),
                    // Ugly. Find a way to cancel auras without Lua.
                    new Action(ret => Lua.DoString("RunMacroText('/cancelaura Blade Flurry');"))
                ),

                Helpers.Spells.CastSelf("Slice and Dice", ret => (Helpers.Rogue.me.ComboPoints >= 1 || 
                                                                 Helpers.Spells.IsAuraActive(Helpers.Rogue.me, "Fury of the Destroyer")) && 
                                                                 Helpers.Spells.GetAuraTimeLeft(Helpers.Rogue.me, "Slice and Dice") < 1),

                Helpers.Spells.Cast("Eviscerate",         ret => (Helpers.Rogue.me.ComboPoints == 5 && (Helpers.Rogue.mCurrentEnergy >= 65 ||
                                                                 Helpers.Rogue.me.HasAura("Adrenaline Rush"))) || 
                                                                 Helpers.Spells.IsAuraActive(Helpers.Rogue.me, "Fury of the Destroyer")),

                Helpers.Spells.CastFocus("Tricks of the Trade", ret => !Helpers.Spells.IsAuraActive(Helpers.Rogue.me, "Tricks of the Trade") && 
                                                                       Helpers.Rogue.mCurrentEnergy < 60),

                new Decorator(ret => Helpers.Rogue.IsCooldownsUsable() && Helpers.Rogue.me.HasAura("Slice and Dice") &&
                                     (Helpers.Spells.IsAuraActive(Helpers.Rogue.me, "Moderate Insight") ||
                                     Helpers.Spells.IsAuraActive(Helpers.Rogue.me, "Deep Insight")) &&
                                     Helpers.Rogue.mCurrentEnergy <= 20,
                    new PrioritySelector(
                        Helpers.Specials.UseSpecialAbilities(),
                        Helpers.Spells.CastSelf("Adrenaline Rush"),
                        Helpers.Spells.CastCooldown("Killing Spree", ret => !Helpers.Rogue.me.HasAura("Adrenaline Rush"))
                    )
                ),

                Helpers.Spells.Cast("Revealing Strike", ret => Helpers.Rogue.me.ComboPoints == 4 && 
                                                               !Helpers.Spells.IsAuraActive(Helpers.Rogue.me.CurrentTarget, "Revealing Strike")),

                Helpers.Spells.Cast("Sinister Strike",  ret => Helpers.Rogue.me.ComboPoints < 5)
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
