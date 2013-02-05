//////////////////////////////////////////////////
//               Raid/Combat.cs                 //
//        Part of RogueBT by kaihaider          //
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
                //Helpers.Movement.MoveToLos(),
                Helpers.Movement.ChkFace(),
                Helpers.Movement.MoveToTarget(),
                Helpers.Spells.ToggleAutoAttack(),
                new Decorator(ret => (Helpers.Aura.Stealth || Helpers.Aura.Vanish) && Helpers.Rogue.mTarget.IsWithinMeleeRange,
                    new PrioritySelector(
                                Helpers.Spells.Cast("Ambush", ret => Helpers.Aura.IsBehind),
                                Helpers.Spells.Cast("Garrote", ret => !Helpers.Aura.IsBehind && !Helpers.Rogue.mTarget.HasAura("Garrote"))
                        )
                ),
                Helpers.Spells.CastCooldown("Feint", ret => (Helpers.Aura.IsTargetCasting == 109034 || Helpers.Aura.IsTargetCasting == 109033) && Helpers.Rogue.mTarget.IsWithinMeleeRange),

                Helpers.Spells.CastSelf("Blade Flurry", ret => Helpers.Rogue.IsAoeUsable() && !Helpers.Aura.BladeFlurry
                                                                && (Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.IsWithinMeleeRange) > 1
                                                               && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 15) < 4)),

                new Decorator(ret => Helpers.Rogue.IsAoeUsable() &&  Helpers.Aura.BladeFlurry && !Helpers.Rogue.me.HasAura("Killing Spree")
                    && (Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 15) < 2 || Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 15) > 3)
                    , // Ugly. Find a way to cancel auras without Lua.
                    new Action(ret => Lua.DoString("RunMacroText('/cancelaura Blade Flurry');"))
                ),

                Helpers.Spells.Cast("Crimson Tempest", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish) && Helpers.Rogue.IsAoeUsable() && Helpers.Rogue.mComboPoints > 4 &&
                                                            Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 3),

                Helpers.Spells.CastSelf("Slice and Dice", ret => (Helpers.Rogue.mComboPoints > 0 || Helpers.Aura.FuryoftheDestroyer)
                                                                 && Helpers.Aura.TimeSliceandDice < 2),

                Helpers.Spells.Cast("Rupture", ret => Helpers.Aura.RevealingStrike && Helpers.Rogue.mComboPoints > 4 && !Helpers.Aura.BladeFlurry
                                 && (!Helpers.Aura.Rupture || Helpers.Aura.TimeRupture < 3) && (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)),

                Helpers.Spells.Cast("Eviscerate", ret => Helpers.Aura.RevealingStrike && (Helpers.Rogue.mComboPoints == 5 && (Helpers.Rogue.mCurrentEnergy >= 65 || Helpers.Aura.AdrenalineRush) || Helpers.Aura.FuryoftheDestroyer)
                                                                  && (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)),

                new Decorator(ret => Helpers.Rogue.IsCooldownsUsable() && Helpers.Aura.SliceandDice
                                     && (Helpers.Aura.ModerateInsight || Helpers.Aura.DeepInsight)
                                     && !(Helpers.Aura.Stealth || Helpers.Aura.Vanish)
                                     && (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)
                                     && Helpers.Rogue.mCurrentEnergy <= 30,
                    new PrioritySelector(
                        Helpers.Specials.UseSpecialAbilities(),
                        Helpers.Spells.CastSelf("Adrenaline Rush"),
                        Helpers.Spells.CastSelf("Shadow Blades"),
                        new Decorator(ret => Helpers.Spells.CanCast("Vanish") && ! Helpers.Aura.AdrenalineRush
                                             && Helpers.Rogue.mCurrentEnergy >= 60 && Helpers.Rogue.mCurrentEnergy <= 100 // check shadow focus adjust
                                             && Helpers.Rogue.mComboPoints != 5 && Helpers.Movement.IsInSafeMeleeRange,
                            new Sequence(
                                Helpers.Spells.CastSelf("Vanish"),
                                Helpers.Rogue.CreateWaitForLagDuration(),
                                new Action(ret => { Helpers.Movement.MoveToTarget(); return RunStatus.Success; }),
                                new DecoratorContinue(ret => Helpers.Aura.IsBehind, Helpers.Spells.Cast("Ambush")),
                                Helpers.Spells.Cast("Garrote", ret => !Helpers.Aura.IsBehind)
                                )
                            )
                    )
                ),
                Helpers.Spells.Cast("Fan of Knives", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish) && Helpers.Rogue.IsAoeUsable()
                                                            && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 3),
                Helpers.Spells.Cast("Revealing Strike", ret => !Helpers.Aura.RevealingStrike && Helpers.Rogue.mCurrentEnergy > 60
                                                        && (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)),
                Helpers.Spells.Cast("Sinister Strike", ret => Helpers.Aura.RevealingStrike && Helpers.Rogue.mComboPoints < 5 
                                                        && (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)),
                Helpers.Spells.Cast("Redirect", ret => Helpers.Rogue.mComboPoints < Helpers.Rogue.me.RawComboPoints),
                new Decorator(ret => Helpers.Rogue.mTarget != null && Helpers.Spells.FindSpell(114014) && Helpers.Rogue.mCurrentEnergy > 20
                    && !Helpers.Aura.Stealth
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
                                new Action(ret => RunStatus.Failure)
                    )
                ),
                Helpers.Spells.CastFocus("Tricks of the Trade", ret => !Helpers.Aura.Tricks && Helpers.Focus.mFocusTarget != null
                                  && Helpers.Rogue.mCurrentEnergy > 40 && Helpers.Rogue.mCurrentEnergy < 75 && Helpers.Rogue.mComboPoints > 1),
                new Decorator(ret => Helpers.Rogue.IsCooldownsUsable() && Helpers.Aura.SliceandDice
                                     && (Helpers.Aura.ModerateInsight || Helpers.Aura.DeepInsight) 
                                     && Helpers.Movement.IsInSafeMeleeRange
                                     && Helpers.Rogue.mCurrentEnergy <= 30 &&
                    
                    !Helpers.Aura.AdrenalineRush && !Helpers.Rogue.mTarget.Name.Equals("Empyreal Focus")
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
                )
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
