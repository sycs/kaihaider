//////////////////////////////////////////////////
//            Raid/Assassination.cs             //
//        Part of RogueBT by kaihaider          //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Linq;
using CommonBehaviors.Actions;
using Styx;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;

namespace RogueBT.Composites.Context.Raid
{

    static class Assassination
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
                                Helpers.Spells.Cast("Mutilate", ret => !Helpers.Aura.IsBehind)
                        )
                ),
                Helpers.Spells.CastCooldown("Feint", ret => (Helpers.Aura.IsTargetCasting == 109034 || Helpers.Aura.IsTargetCasting == 109033)
                    && Helpers.Movement.IsInSafeMeleeRange),
                Helpers.Spells.Cast("Envenom", ret => Helpers.Aura.DeadlyPoison && (Helpers.Aura.TimeSliceandDice <= 3 && Helpers.Aura.SliceandDice && Helpers.Rogue.mComboPoints > 0)
                                    && (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)),
                Helpers.Spells.Cast("Dispatch", ret => Helpers.Rogue.mComboPoints != 5 && Helpers.Aura.Blindside && Helpers.Rogue.mCurrentEnergy < 90
                                        && (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)),
                Helpers.Spells.Cast("Rupture", ret => (Helpers.Aura.TimeRupture < 4 || !Helpers.Aura.Rupture) 
                                                 && (Helpers.Rogue.mComboPoints >= 4 && Helpers.Rogue.mTargetHP > 35 || Helpers.Rogue.mComboPoints == 5)
                                                                 && (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)),
                Helpers.Spells.CastSelf("Slice and Dice", ret => Helpers.Aura.TimeSliceandDice < 2 && Helpers.Rogue.mComboPoints > 0),
                Helpers.Spells.Cast("Envenom", ret => Helpers.Aura.DeadlyPoison && Helpers.Aura.FuryoftheDestroyer && (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)),
                new Decorator(ret => Helpers.Rogue.IsCooldownsUsable() && !(Helpers.Aura.Stealth || Helpers.Aura.Vanish)
                    && Helpers.Rogue.mCurrentEnergy < 85 && Helpers.Rogue.mCurrentEnergy > 45
                                    && Helpers.Aura.Rupture && Helpers.Aura.SliceandDice && Helpers.Movement.IsInSafeMeleeRange,
                    new PrioritySelector(
                        Helpers.Specials.UseSpecialAbilities(ret => Helpers.Aura.Vendetta ||
                                                                    Helpers.Aura.TimeVendetta > 0),
                        Helpers.Spells.CastCooldown("Vendetta"),
                        Helpers.Spells.CastSelf("Shadow Blades"),

                        new Decorator(ret => Helpers.Spells.CanCast("Vanish") && Helpers.Aura.TimeSliceandDice > 6
                                             && Helpers.Rogue.mCurrentEnergy >= 60 && Helpers.Rogue.mCurrentEnergy <= 100 &&
                                             Helpers.Rogue.mComboPoints != 5 && Helpers.Movement.IsInSafeMeleeRange && !Helpers.Rogue.me.HasAura("Shadow Blades"),
                            new Sequence(
                                Helpers.Spells.CastSelf("Vanish"),
                                Helpers.Rogue.CreateWaitForLagDuration(),
                                new Action(ret => {Helpers.Movement.MoveToTarget(); return RunStatus.Success; }),
                                new DecoratorContinue(ret => Helpers.Aura.IsBehind, Helpers.Spells.Cast("Ambush")),
                                Helpers.Spells.Cast("Mutilate", ret => !Helpers.Aura.IsBehind)
                                )
                            )
                    )
                ),
                Helpers.Spells.Cast("Envenom", ret => Helpers.Aura.DeadlyPoison && Helpers.Rogue.mComboPoints > 0
                                                       && (Helpers.Rogue.mComboPoints == 4 && Helpers.Rogue.mTargetHP >= 35 && !Helpers.Aura.Blindside
                                                       || Helpers.Rogue.mComboPoints > 4 && Helpers.Rogue.mTargetHP >= 35
                                                       || (Helpers.Rogue.mCurrentEnergy >= 55 && Helpers.Rogue.mComboPoints == 5 && Helpers.Rogue.mTargetHP < 35))
                                                        && (!Helpers.Aura.Envenom || Helpers.Rogue.mCurrentEnergy > 90)
                                                        && (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)),
                Helpers.Spells.Cast("Dispatch", ret => (Helpers.Rogue.mTargetHP < 35 || Helpers.Aura.Blindside)
                                                        && ((!Helpers.Aura.Rupture || (Helpers.Rogue.mCurrentEnergy >= 80 || Helpers.Aura.Envenom || Helpers.Aura.Blindside)))
                                                       && (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)),
                Helpers.Spells.Cast("Fan of Knives", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish)
                                                        && Helpers.Rogue.IsAoeUsable() && (Helpers.Aura.Envenom || Helpers.Rogue.mCurrentEnergy > 80)
                                                            && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 1),
                Helpers.Spells.Cast("Mutilate", ret => (!Helpers.Aura.Rupture || (Helpers.Rogue.mComboPoints < 5 && (Helpers.Rogue.mCurrentEnergy >= 85 || Helpers.Aura.Envenom)))
                                                       && (Helpers.Movement.IsInSafeMeleeRange || !Settings.Mode.mUseMovement)),
                Helpers.Spells.Cast("Redirect", ret => Helpers.Rogue.mComboPoints < Helpers.Rogue.me.RawComboPoints),
                new Decorator(ret => Helpers.Rogue.mTarget != null && Helpers.Spells.FindSpell(114014) && Helpers.Rogue.mCurrentEnergy > 70
                    && !(Helpers.Aura.Stealth || Helpers.Aura.Vanish) && (Helpers.Rogue.mTarget.Distance > 10 && Helpers.Rogue.mTarget.Distance < 30
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
                                  && Helpers.Rogue.mCurrentEnergy > 40 && Helpers.Rogue.mCurrentEnergy < 75 && Helpers.Rogue.mComboPoints > 1)
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
