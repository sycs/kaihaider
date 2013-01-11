//////////////////////////////////////////////////
//               Raid/Combat.cs                 //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Linq;
using Styx.WoWInternals;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;

namespace RogueBT.Composites.Context.Raid
{
    class Combat
    {
        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(
                Helpers.Movement.PleaseStop(),
                //Helpers.Target.EnsureValidTarget(),
                Helpers.Movement.MoveToLos(),
                //Helpers.Movement.ChkFace(),
                Helpers.Spells.ToggleAutoAttack(),

                Helpers.Spells.CastCooldown("Feint", ret => (Helpers.Aura.IsTargetCasting == 109034 || Helpers.Aura.IsTargetCasting == 109033) &&
                    Helpers.Rogue.mTarget.IsWithinMeleeRange),


                Helpers.Spells.CastSelf("Blade Flurry", ret => Helpers.Rogue.IsAoeUsable() && !Helpers.Aura.BladeFlurry
                                                                && (Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.IsWithinMeleeRange) > 1
                                                               && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 15) < 3)),

                new Decorator(ret => Helpers.Rogue.IsAoeUsable() &&  Helpers.Aura.BladeFlurry
                    && (Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 15) < 2 || Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 15) > 3)
                    , // Ugly. Find a way to cancel auras without Lua.
                    new Action(ret => Lua.DoString("RunMacroText('/cancelaura Blade Flurry');"))
                ),


                Helpers.Spells.Cast("Crimson Tempest", ret => Helpers.Rogue.IsAoeUsable() && Helpers.Rogue.mComboPoints > 4 &&
                                                            Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 3),

                Helpers.Spells.CastSelf("Slice and Dice", ret => (Helpers.Rogue.mComboPoints >= 1 || 
                                                                 Helpers.Aura.FuryoftheDestroyer) && 
                                                                 Helpers.Aura.TimeSliceandDice < 1),

		        Helpers.Spells.Cast("Rupture",         ret => Helpers.Rogue.mComboPoints > 4 && 
								(Helpers.Aura.TimeRupture < 3|| !Helpers.Aura.Rupture)),

                Helpers.Spells.Cast("Eviscerate",         ret => (Helpers.Rogue.mComboPoints == 5 && (Helpers.Rogue.mCurrentEnergy >= 65 ||
                                                                 Helpers.Aura.AdrenalineRush)) || 
                                                                 Helpers.Aura.FuryoftheDestroyer),

                Helpers.Spells.CastFocus("Tricks of the Trade", ret => !Helpers.Aura.Tricks  && Helpers.Focus.mFocusTarget!=null &&
                                                                       Helpers.Rogue.mCurrentEnergy > 40 && Helpers.Rogue.mCurrentEnergy < 75 &&
Helpers.Rogue.mComboPoints > 1),


                new Decorator(ret => Helpers.Rogue.IsCooldownsUsable() && Helpers.Aura.SliceandDice &&
                                     (Helpers.Aura.ModerateInsight ||
                                     Helpers.Aura.DeepInsight) &&
                                     Helpers.Rogue.mCurrentEnergy <= 30,
                    new PrioritySelector(
                        Helpers.Specials.UseSpecialAbilities(),
                        Helpers.Spells.CastSelf("Adrenaline Rush"),
                        Helpers.Spells.CastCooldown("Killing Spree", ret => !Helpers.Aura.AdrenalineRush)
                    )
                ),

                Helpers.Spells.Cast("Revealing Strike", ret => !Helpers.Aura.RevealingStrike && Helpers.Movement.IsInSafeMeleeRange),

                Helpers.Spells.Cast("Sinister Strike", ret => Helpers.Rogue.mComboPoints < 5 && Helpers.Movement.IsInSafeMeleeRange),

                Helpers.Spells.CastFocus("Tricks of the Trade", ret => !Helpers.Aura.Tricks && Helpers.Focus.mFocusTarget != null &&
                                                                       Helpers.Rogue.mCurrentEnergy > 50 && Helpers.Rogue.mCurrentEnergy < 95
                                                                       && Helpers.Rogue.mComboPoints > 1),


                Helpers.Movement.MoveToTarget(),

                Helpers.Spells.Cast("Redirect", ret => Helpers.Rogue.mComboPoints < Helpers.Rogue.me.RawComboPoints)
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
