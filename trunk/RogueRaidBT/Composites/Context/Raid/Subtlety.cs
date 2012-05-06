//////////////////////////////////////////////////
//              Raid/Subtlety.cs                //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System;
using CommonBehaviors.Actions;
using Styx;
using TreeSharp;
using Action = TreeSharp.Action;

namespace RogueRaidBT.Composites.Context.Raid
{
    static class Subtlety
    {
        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(
                Helpers.Spells.ToggleAutoAttack(ret => !Helpers.Aura.Vanish && !Helpers.Aura.IsTargetDisoriented && !Helpers.Aura.IsTargetSapped),

                Helpers.Spells.CastCooldown("Feint", ret => (Helpers.Aura.IsTargetCasting == 109034 || Helpers.Aura.IsTargetCasting == 109033) &&
                    Helpers.Rogue.mTarget.IsWithinMeleeRange),

                Helpers.Spells.Cast("Redirect", ret => Helpers.Rogue.mComboPoints < StyxWoW.Me.RawComboPoints),

                new Decorator(ret => Helpers.Rogue.mComboPoints == 5 || Helpers.Aura.FuryoftheDestroyer,
                    new PrioritySelector(
                        Helpers.Spells.Cast("Rupture",            ret => Helpers.Area.IsCurTargetSpecial() &&
                                                                         Helpers.Aura.FindWeakness &&
                                                                         !Helpers.Aura.Rupture),
                        Helpers.Spells.CastSelf("Slice and Dice", ret => Helpers.Aura.TimeSliceandDice < 3),
                        Helpers.Spells.Cast("Rupture",            ret => Helpers.Area.IsCurTargetSpecial() &&
                                                                         !Helpers.Aura.Rupture),
                        Helpers.Spells.CastSelf("Recuperate",     ret => Helpers.Aura.TimeRecuperate < 3),
                        Helpers.Spells.Cast("Eviscerate",         ret => Helpers.Aura.FindWeakness || 
                                                                         Helpers.Aura.FuryoftheDestroyer ||
                                                                         Helpers.Rogue.mCurrentEnergy >= 60 || (Helpers.Area.IsCurTargetSpecial() && 
                                                                         Helpers.Aura.TimeRecuperate < 3))
                    )
                ),

                Helpers.Spells.CastFocus("Tricks of the Trade", ret => !Helpers.Aura.Tricks && 
                                                                       Helpers.Rogue.mCurrentEnergy < 60),

                Helpers.Spells.CastCooldown("Premeditation", ret => Helpers.Rogue.mComboPoints <= 3 && (Helpers.Aura.Stealth || 
                                                                    Helpers.Aura.ShadowDance || Helpers.Aura.Vanish)),

                Helpers.Specials.UseSpecialAbilities(ret => Helpers.Aura.ShadowDance ||
                                                            Helpers.Spells.GetSpellCooldown("Shadow Dance") >= 10),

                new Decorator(ret => Helpers.Rogue.IsCooldownsUsable() && 
                                     Helpers.Rogue.mComboPoints == 0 &&
                                     Helpers.Rogue.mCurrentEnergy >= 50 &&
                                     !(Helpers.Spells.GetSpellCooldown("Premeditation") > 0),
                    new PrioritySelector(
                        new Decorator(ret => Helpers.Spells.CanCast("Shadow Dance"),
                            new Sequence(
                                Helpers.Spells.CastSelf("Shadow Dance"),
                                new WaitContinue(TimeSpan.FromSeconds(0.5), ret => false, new ActionAlwaysSucceed())
                            )
                        ),

                        new Decorator(ret => Helpers.Rogue.IsCooldownsUsable() &&
                                             !Helpers.Spells.IsAuraActive(StyxWoW.Me, "Shadow Dance") &&
                                             Helpers.Spells.GetSpellCooldown("Shadow Dance") > 0 &&
                                             Helpers.Spells.CanCast("Vanish"),
                            new Sequence(
                                Helpers.Spells.CastSelf("Vanish"),
                                new WaitContinue(TimeSpan.FromSeconds(1), ret => false, new ActionAlwaysSucceed()),
                                Helpers.Spells.CastCooldown("Premeditation"),
                                Helpers.Spells.Cast("Ambush", ret => Helpers.Aura.IsBehind)
                            )
                        ),

                        Helpers.Spells.CastSelf("Preparation", ret => Helpers.Rogue.IsCooldownsUsable() &&
                                                                      Helpers.Spells.GetSpellCooldown("Vanish") > 30)
                    )
                ),

                // CP Builders
                new Decorator(ret => Helpers.Rogue.mComboPoints != 5 && (Helpers.Rogue.mComboPoints < 4 ||
                                     (Helpers.Rogue.mComboPoints == 4 && (Helpers.Rogue.mCurrentEnergy >= 90 ||
                                     Helpers.Aura.TimeRupture < 3 ||
                                     Helpers.Aura.ShadowDance))),
                    new PrioritySelector(
                        Helpers.Spells.Cast("Ambush",     ret => Helpers.Aura.IsBehind && 
                                                                 (Helpers.Aura.Stealth || 
                                                                    Helpers.Aura.ShadowDance || Helpers.Aura.Vanish)),
                        Helpers.Spells.Cast("Hemorrhage", ret => Helpers.Aura.TimeRupture < 3),
                        Helpers.Spells.Cast("Backstab",   ret => Helpers.Aura.IsBehind),
                        Helpers.Spells.Cast("Hemorrhage", ret => !Helpers.Aura.IsBehind)
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
