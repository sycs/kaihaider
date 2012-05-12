//////////////////////////////////////////////////
//               Raid/Combat.cs                 //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Linq;
using Styx.WoWInternals;
using TreeSharp;
using Action = TreeSharp.Action;

namespace RogueRaidBT.Composites.Context.Raid
{
    class Combat
    {
        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(
                Helpers.Spells.ToggleAutoAttack(ret => !Helpers.Aura.Vanish && !Helpers.Aura.IsTargetDisoriented && !Helpers.Aura.IsTargetSapped),

                Helpers.Spells.CastCooldown("Feint", ret => (Helpers.Aura.IsTargetCasting == 109034 || Helpers.Aura.IsTargetCasting == 109033) &&
                    Helpers.Rogue.mTarget.IsWithinMeleeRange),

                Helpers.Spells.Cast("Redirect", ret => Helpers.Rogue.mComboPoints < Helpers.Rogue.mRawComboPoints),


                Helpers.Spells.CastSelf("Blade Flurry", ret => Helpers.Rogue.IsAoeUsable() && !Helpers.Aura.BladeFlurry &&
                                                               Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.IsWithinMeleeRange) >= 2),

                new Decorator(ret => Helpers.Rogue.IsAoeUsable() && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 15) < 2 && 
                                     Helpers.Aura.BladeFlurry,
                    // Ugly. Find a way to cancel auras without Lua.
                    new Action(ret => Lua.DoString("RunMacroText('/cancelaura Blade Flurry');"))
                ),

                Helpers.Spells.CastSelf("Slice and Dice", ret => (Helpers.Rogue.mComboPoints >= 1 || 
                                                                 Helpers.Aura.FuryoftheDestroyer) && 
                                                                 Helpers.Aura.TimeSliceandDice < 1),

		Helpers.Spells.Cast("Rupture",         ret => (Helpers.Rogue.mComboPoints > 3 && (Helpers.Rogue.mCurrentEnergy >= 65 ||
                                                                 Helpers.Aura.AdrenalineRush)) &&
								(Helpers.Aura.TimeRupture < 1|| !Helpers.Aura.Rupture)),

                Helpers.Spells.Cast("Eviscerate",         ret => (Helpers.Rogue.mComboPoints == 5 && (Helpers.Rogue.mCurrentEnergy >= 65 ||
                                                                 Helpers.Aura.AdrenalineRush)) || 
                                                                 Helpers.Aura.FuryoftheDestroyer),

                Helpers.Spells.CastFocus("Tricks of the Trade", ret => !Helpers.Aura.Tricks  && Helpers.Focus.mFocusTarget!=null &&
                                                                       Helpers.Rogue.mCurrentEnergy > 40 && Helpers.Rogue.mCurrentEnergy < 75 &&
Helpers.Rogue.mComboPoints > 1),


                new Decorator(ret => Helpers.Rogue.IsCooldownsUsable() && Helpers.Aura.SliceandDice &&
                                     (Helpers.Aura.ModerateInsight ||
                                     Helpers.Aura.DeepInsight) &&
                                     Helpers.Rogue.mCurrentEnergy <= 20,
                    new PrioritySelector(
                        Helpers.Specials.UseSpecialAbilities(),
                        Helpers.Spells.CastSelf("Adrenaline Rush"),
                        Helpers.Spells.CastCooldown("Killing Spree", ret => !Helpers.Aura.AdrenalineRush)
                    )
                ),

                Helpers.Spells.Cast("Revealing Strike", ret => Helpers.Rogue.mComboPoints == 4 &&
                                                               !Helpers.Aura.RevealingStrike),

                Helpers.Spells.Cast("Sinister Strike",  ret => Helpers.Rogue.mComboPoints < 5)
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
