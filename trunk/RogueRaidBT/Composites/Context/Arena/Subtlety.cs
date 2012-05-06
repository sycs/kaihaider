//////////////////////////////////////////////////
//              Arena/Subtlety.cs               //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System;
using System.Linq;
using CommonBehaviors.Actions;
using Styx;
using Styx.Combat.CombatRoutine;
using TreeSharp;
using Action = TreeSharp.Action;

namespace RogueRaidBT.Composites.Context.Arena
{
    static class Subtlety
    {
        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(
                /**
                 * unit.face()
                 * 
                 */


                Helpers.Spells.ToggleAutoAttack(ret => !Helpers.Aura.Vanish && !Helpers.Aura.IsTargetDisoriented && !Helpers.Aura.IsTargetSapped),

                Helpers.Spells.CastCooldown("Feint", ret => (Helpers.Aura.IsTargetCasting == 46924 || Helpers.Aura.IsTargetCasting == 1680) && 
                    Helpers.Rogue.mTarget.IsWithinMeleeRange),

                Helpers.Spells.CastCooldown("Sap", ret => (Helpers.Aura.ShadowDance || Helpers.Aura.Stealth) && !Helpers.Aura.IsTargetSapped &&
                    Helpers.Rogue.mTarget != Helpers.Focus.rawFocusTarget && !Helpers.Rogue.mTarget.Combat ),

                Helpers.Rogue.TryToInterrupt(ret => Helpers.Aura.IsTargetCasting != 0 && 
                    ((Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds <= 0.6 &&
                    Helpers.Rogue.mTarget.CurrentCastTimeLeft.TotalSeconds >= 0.1 )||
                    (Helpers.Aura.IsTargetCasting == 740 || Helpers.Aura.IsTargetCasting == 47540 ||
                    Helpers.Aura.IsTargetCasting == 64843 || Helpers.Aura.IsTargetCasting == 12051 ||
                    Helpers.Aura.IsTargetCasting == 118
                    ))),

                Helpers.Rogue.TryToInterruptFocus(ret => Helpers.Focus.rawFocusTarget != null && Helpers.Focus.rawFocusTarget != Helpers.Rogue.mTarget &&
                    !Helpers.Focus.rawFocusTarget.IsFriendly && Helpers.Focus.rawFocusTarget.IsWithinMeleeRange &&
                    Helpers.Focus.rawFocusTarget.IsCasting ),/*&&
                  
                    ((Helpers.Focus.rawFocusTarget.CurrentCastTimeLeft.TotalSeconds <= 0.6 &&
                    Helpers.Focus.rawFocusTarget.CurrentCastTimeLeft.TotalSeconds >= 0.1) ||
                    (Helpers.Aura.IsFocusCasting ==  || Helpers.Aura.IsFocusCasting == 
                    ))**/

		        Helpers.Spells.CastSelf("Recuperate",     ret => Helpers.Rogue.mComboPoints > 2 && Helpers.Rogue.mHP < 95 &&
                                Helpers.Aura.TimeRecuperate< 3), // Helpers.Spells.GetAuraTimeLeft(StyxWoW.Me, "Recuperate") 

                new Decorator(ret => !Helpers.Aura.IsTargetInvulnerable && !Helpers.Aura.IsTargetSapped && !Helpers.Aura.IsTargetDisoriented && 
                                        (Helpers.Rogue.mComboPoints == 5 || Helpers.Aura.FuryoftheDestroyer),
                    new PrioritySelector(
                        Helpers.Spells.CastSelf("Slice and Dice", ret => Helpers.Aura.TimeSliceandDice < 3), 

                        Helpers.Spells.Cast("Rupture", ret =>  !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                                    !Helpers.Aura.FindWeakness && !Helpers.Aura.Rupture),


                        Helpers.Spells.Cast("Eviscerate", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish ) && !Helpers.Aura.IsTargetSapped &&
                                (Helpers.Aura.FindWeakness || Helpers.Aura.FuryoftheDestroyer || Helpers.Aura.ShadowDance)),

                        Helpers.Spells.CastCooldown("Kidney Shot", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish) &&
                            Helpers.Focus.rawFocusTarget != null &&  Helpers.Focus.rawFocusTarget == Helpers.Rogue.mTarget &&
                            !Helpers.Rogue.mTarget.Silenced && !Helpers.Rogue.mTarget.Stunned &&
                            !Helpers.Aura.IsTargetImmuneStun),

			            Helpers.Spells.Cast("Eviscerate", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish))

                    )
                ),

                new Decorator(ret => Helpers.Rogue.mCurrentEnergy > 30  && Helpers.Rogue.mTarget != null && !Helpers.Aura.IsTargetInvulnerable && !Helpers.Aura.IsTargetDisoriented
                    && !(Helpers.Aura.Stealth || Helpers.Aura.Vanish) && !Helpers.Aura.IsTargetSapped,
                    new PrioritySelector(
                         Helpers.Spells.Cast("Deadly Throw", ret => !Helpers.Rogue.mTarget.IsWithinMeleeRange
                            && Helpers.Rogue.mComboPoints > 0 && Helpers.Rogue.mComboPoints < 3 &&
                                                    Helpers.Rogue.mCurrentEnergy >= 70 && Helpers.Aura.CripplingPoison),

                         Helpers.Spells.CastCooldown("Shiv", ret => Helpers.Aura.ShouldShiv &&
                                        !(Helpers.Aura.ShadowDance) && Helpers.Rogue.mCurrentEnergy < 100),

                         Helpers.Spells.CastCooldown("Dismantle", ret => (Helpers.Rogue.mHP < 65 || (Helpers.Focus.rawFocusTarget != null && Helpers.Focus.rawFocusTarget == Helpers.Rogue.mTarget)) &&
                             Helpers.Rogue.mTarget.IsPlayer && Helpers.Rogue.mCurrentEnergy < 100 && !(Helpers.Aura.ShadowDance) 
                            )
                        )
                ),


                Helpers.Spells.CastCooldown("Premeditation", ret => Helpers.Rogue.mComboPoints <= 3 && !Helpers.Aura.IsTargetSapped && (Helpers.Aura.Stealth ||
                                                                    Helpers.Aura.ShadowDance || Helpers.Aura.Vanish)),
                                 
                // CP Builders
                new Decorator(ret => Helpers.Rogue.mTarget != null && !Helpers.Aura.IsTargetDisoriented && 
                                     !Helpers.Aura.IsTargetInvulnerable &&
                                     Helpers.Rogue.mComboPoints != 5 && (Helpers.Rogue.mComboPoints < 4 ||
                                     (Helpers.Rogue.mComboPoints == 4 && (Helpers.Rogue.mCurrentEnergy >= 90 ||
                                     Helpers.Aura.TimeRupture < 3 ||
                                     Helpers.Aura.ShadowDance))) && !Helpers.Aura.IsTargetSapped,
                    new PrioritySelector(

                        Helpers.Spells.Cast("Garrote",     ret =>  (Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                            Helpers.Focus.rawFocusTarget != null && Helpers.Focus.rawFocusTarget == Helpers.Rogue.mTarget && 
                                !Helpers.Rogue.mTarget.Silenced && !Helpers.Rogue.mTarget.Stunned && Helpers.Aura.IsBehind 
                               ),

                        Helpers.Spells.Cast("Ambush",     ret =>  (Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                            Helpers.Aura.IsBehind ),
                        Helpers.Spells.CastCooldown("Cheap Shot", ret => (Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                                                             !Helpers.Rogue.mTarget.Stunned  && !Helpers.Rogue.mTarget.Silenced ),
                        Helpers.Spells.Cast("Hemorrhage", ret => !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) 
                                                    && Helpers.Aura.TimeHemorrhage < 3),
			            Helpers.Spells.Cast("Fan of Knives", ret => Helpers.Rogue.IsAoeUsable() && 
                                                            Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 6),
                        Helpers.Spells.Cast("Backstab",   ret => ! (Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance) &&
                                                    Helpers.Rogue.mCurrentEnergy > 60 && Helpers.Aura.IsBehind),
                        Helpers.Spells.Cast("Hemorrhage", ret => Helpers.Rogue.mCurrentEnergy > 70 &&
                        !(Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance)
                                                                 && !Helpers.Aura.IsBehind)
                    )
                ),

                Helpers.Spells.CastFocus("Tricks of the Trade", ret => !Helpers.Aura.Tricks &&
                                                                       Helpers.Rogue.mCurrentEnergy < 60)

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
