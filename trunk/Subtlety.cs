//////////////////////////////////////////////////
//              Raid/Subtlety.cs                //
//      Part of PvP_RaidBT by fiftypence        //
//////////////////////////////////////////////////

using System;
using CommonBehaviors.Actions;
using Styx;
using TreeSharp;
using Action = TreeSharp.Action;

namespace PvP_RaidBT.Composites.Context.Raid
{
    static class Subtlety
    {
        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(


                Helpers.Spells.ToggleAutoAttack(ret => !Helpers.Aura.Vanish),

		        Helpers.Spells.CastSelf("Recuperate",     ret => StyxWoW.Me.ComboPoints > 2 && StyxWoW.Me.HealthPercent < 95 &&
                                Helpers.Aura.TimeRecuperate< 3), // Helpers.Spells.GetAuraTimeLeft(StyxWoW.Me, "Recuperate") 


                new Decorator(ret => StyxWoW.Me.ComboPoints == 5 || Helpers.Aura.FuryoftheDestroyer,
                    new PrioritySelector(
                        Helpers.Spells.CastSelf("Slice and Dice", ret => Helpers.Aura.TimeSliceandDice < 3), 

                        Helpers.Spells.Cast("Rupture", ret =>  !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                                    !Helpers.Aura.FindWeakness && !Helpers.Aura.Rupture),

                        Helpers.Spells.Cast("Eviscerate", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish ) && 
                                (Helpers.Aura.FindWeakness || Helpers.Aura.FuryoftheDestroyer || Helpers.Rogue.mCurrentEnergy >= 70)),

                        Helpers.Spells.CastCooldown("Kidney Shot", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish) &&
                            !StyxWoW.Me.CurrentTarget.Silenced && !StyxWoW.Me.CurrentTarget.Stunned),

			            Helpers.Spells.Cast("Eviscerate", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish))

                    )
                ),

                Helpers.Spells.Cast("Deadly Throw", ret => StyxWoW.Me.CurrentTarget != null && !StyxWoW.Me.CurrentTarget.IsWithinMeleeRange 
							&& StyxWoW.Me.ComboPoints > 0 && StyxWoW.Me.ComboPoints < 3 && !(Helpers.Aura.Stealth || Helpers.Aura.Vanish ) &&
                                                    Helpers.Rogue.mCurrentEnergy >= 70 && Helpers.Spells.IsAuraActive(StyxWoW.Me, "Crippling Poison")),

                Helpers.Spells.CastFocus("Tricks of the Trade", ret => !Helpers.Aura.Tricks && 
                                                                       Helpers.Rogue.mCurrentEnergy < 60),

                Helpers.Spells.CastCooldown("Premeditation", ret => StyxWoW.Me.ComboPoints <= 3 && (StyxWoW.Me.HasAura("Stealth") ||
                                                                    Helpers.Aura.ShadowDance || Helpers.Aura.Vanish)),

		        Helpers.Spells.CastCooldown("Shiv", ret => Helpers.Rogue.mCurrentEnergy > 40 && Helpers.Rogue.mCurrentEnergy < 100 &&
                                 !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                                Helpers.Aura.ShouldShiv),

                Helpers.Spells.CastCooldown("Dismantle", ret => //StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.CurrentTarget.is &&
                                                Helpers.Rogue.mCurrentEnergy > 40 && Helpers.Rogue.mCurrentEnergy < 100 &&
                                 				!(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) && 
								(StyxWoW.Me.HealthPercent < 65 || 
				(StyxWoW.Me.FocusedUnit != null && StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.FocusedUnit == StyxWoW.Me.CurrentTarget))),

                                 
                // CP Builders
                new Decorator(ret => StyxWoW.Me.ComboPoints != 5 && (StyxWoW.Me.ComboPoints < 4 ||
                                     (StyxWoW.Me.ComboPoints == 4 && (Helpers.Rogue.mCurrentEnergy >= 90 ||
                                     Helpers.Aura.TimeRupture < 3 ||
                                     Helpers.Aura.ShadowDance))),
                    new PrioritySelector(

                        Helpers.Spells.Cast("Garrote",     ret => StyxWoW.Me.FocusedUnit != null && StyxWoW.Me.CurrentTarget != null &&
                                StyxWoW.Me.FocusedUnit == StyxWoW.Me.CurrentTarget && !StyxWoW.Me.CurrentTarget.Silenced && !StyxWoW.Me.CurrentTarget.Stunned &&
								Helpers.Rogue.IsBehindUnit(StyxWoW.Me.CurrentTarget) && //!Helpers.Spells.IsAuraActive(StyxWoW.Me, 1330) &&
                                                                 (Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance)),

                        Helpers.Spells.Cast("Ambush",     ret => Helpers.Rogue.IsBehindUnit(StyxWoW.Me.CurrentTarget) &&
                                                                 (Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance)),
                        Helpers.Spells.CastCooldown("Cheap Shot", ret => (Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                                                             !StyxWoW.Me.CurrentTarget.Stunned  && !StyxWoW.Me.CurrentTarget.Silenced ),
                        Helpers.Spells.Cast("Hemorrhage", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) 
                                                    && Helpers.Aura.TimeHemorrhage < 3),
                        Helpers.Spells.Cast("Backstab",   ret => ! (Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                                                    Helpers.Rogue.mCurrentEnergy > 60 && Helpers.Rogue.IsBehindUnit(StyxWoW.Me.CurrentTarget)),
                        Helpers.Spells.Cast("Hemorrhage", ret => Helpers.Rogue.mCurrentEnergy > 60 &&
                        !(Helpers.Aura.Stealth || Helpers.Spells.IsAuraActive(StyxWoW.Me, "Vanish") || Helpers.Aura.ShadowDance)
                                                                 && !Helpers.Rogue.IsBehindUnit(StyxWoW.Me.CurrentTarget))
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
