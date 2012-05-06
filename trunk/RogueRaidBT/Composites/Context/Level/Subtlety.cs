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
using Styx.WoWInternals;

namespace RogueRaidBT.Composites.Context.Level
{
    static class Subtlety
    {
        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(


                Helpers.Spells.ToggleAutoAttack(ret => !Helpers.Aura.Vanish),

		        Helpers.Spells.CastSelf("Recuperate",     ret => Helpers.Rogue.mComboPoints > 2 && Helpers.Rogue.mHP < 95 &&
                                Helpers.Aura.TimeRecuperate< 3), // Helpers.Spells.GetAuraTimeLeft(StyxWoW.Me, "Recuperate") 


                new Decorator(ret => Helpers.Rogue.mComboPoints == 5 || Helpers.Aura.FuryoftheDestroyer,
                    new PrioritySelector(
                        Helpers.Spells.CastSelf("Slice and Dice", ret => Helpers.Aura.TimeSliceandDice < 3), //Helpers.Spells.GetAuraTimeLeft(StyxWoW.Me, "Slice and Dice") 

                        Helpers.Spells.Cast("Rupture", ret => !Helpers.Aura.FindWeakness &&
                                                                         !Helpers.Aura.Rupture),

                        Helpers.Spells.Cast("Eviscerate", ret => Helpers.Aura.FindWeakness ||
                                                                         Helpers.Aura.FuryoftheDestroyer ||
                                                                         Helpers.Rogue.mCurrentEnergy >= 70),
                        
                        Helpers.Spells.CastCooldown("Kidney Shot", ret => true)
                    )
                ),

                Helpers.Spells.Cast("Deadly Throw", ret => Helpers.Rogue.mTarget != null && Helpers.Rogue.mComboPoints > 0 && Helpers.Rogue.mComboPoints < 3 && 
                                                    Helpers.Rogue.mCurrentEnergy >= 70 && !Helpers.Rogue.mTarget.IsWithinMeleeRange),

                Helpers.Spells.CastFocus("Tricks of the Trade", ret => !Helpers.Aura.Tricks && 
                                                                       Helpers.Rogue.mCurrentEnergy < 60),

                Helpers.Spells.CastCooldown("Premeditation", ret => Helpers.Rogue.mComboPoints <= 3 && (Helpers.Aura.Stealth ||
                                                                    Helpers.Aura.ShadowDance || Helpers.Aura.Vanish)),

		Helpers.Spells.CastCooldown("Shiv", ret => Helpers.Rogue.mCurrentEnergy > 40 && Helpers.Rogue.mCurrentEnergy < 100 &&
                                 !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                                Helpers.Aura.ShouldShiv),

		Helpers.Spells.CastCooldown("Dismantle", ret => Helpers.Rogue.mCurrentEnergy > 40 && Helpers.Rogue.mCurrentEnergy < 100 &&
                                 !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) && Helpers.Rogue.mHP < 65),

                

                // CP Builders
                new Decorator(ret => Helpers.Rogue.mComboPoints != 5 && (Helpers.Rogue.mComboPoints < 4 ||
                                     (Helpers.Rogue.mComboPoints == 4 && (Helpers.Rogue.mCurrentEnergy >= 90 ||
                                     Helpers.Aura.TimeRupture < 3 ||
                                     Helpers.Aura.ShadowDance))),
                    new PrioritySelector(
                        Helpers.Spells.Cast("Ambush",     ret => Helpers.Aura.IsBehind &&
                                                                 (Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance)),
                        Helpers.Spells.CastCooldown("Cheap Shot", ret => (Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance)),
                        Helpers.Spells.Cast("Hemorrhage", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) 
                                                    && Helpers.Aura.TimeHemorrhage < 3),
                        Helpers.Spells.Cast("Backstab",   ret => ! (Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) 
                                                    && Helpers.Aura.IsBehind),
                        Helpers.Spells.Cast("Hemorrhage", ret => Helpers.Rogue.mCurrentEnergy > 60 && 
						!(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance)
                                                                 && !Helpers.Aura.IsBehind)
                    )
                )
            );
        }

        static public Composite BuildPullBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret => StyxWoW.Me.Mounted,
                    new Action(ret => Lua.DoString("Dismount()"))
                ),

                Helpers.Spells.CastSelf("Stealth", ret => !StyxWoW.Me.HasAura("Stealth")),

                Helpers.Movement.MoveToAndFaceUnit(ret => StyxWoW.Me.CurrentTarget),
                Helpers.Spells.Cast("Ambush", ret => StyxWoW.Me.HasAura("Stealth") && Helpers.Aura.IsBehind),
                Helpers.Spells.Cast("Cheap Shot", ret => StyxWoW.Me.HasAura("Stealth")),
                Helpers.Spells.Cast("Hemorrhage"),
                Helpers.Spells.Cast("Sinister Strike")
            );
        }

        static public Composite BuildBuffBehavior()
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
