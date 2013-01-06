//////////////////////////////////////////////////
//            Raid/Assassination.cs             //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Linq;
using Styx;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;

namespace RogueBT.Composites.Context.Arena 
{
    static class Assassination

    {
        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(
                Helpers.Spells.ToggleAutoAttack(ret => !Helpers.Aura.Vanish && !Helpers.Aura.IsTargetDisoriented && !Helpers.Aura.IsTargetSapped),

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


                Helpers.Spells.Cast("Redirect",           ret => StyxWoW.Me.ComboPoints < StyxWoW.Me.RawComboPoints),

                Helpers.Spells.Cast("Envenom",            ret => Helpers.Spells.IsAuraActive(StyxWoW.Me, "Fury of the Destroyer") &&
                                                                Helpers.Aura.DeadlyPoison),

                Helpers.Spells.CastSelf("Slice and Dice", ret => !Helpers.Spells.IsAuraActive(StyxWoW.Me, "Slice and Dice") &&
                                                                 StyxWoW.Me.ComboPoints >= 1),


		Helpers.Spells.Cast("Garrote", ret => StyxWoW.Me.HasAura("Vanish")),

                Helpers.Spells.Cast("Rupture",            ret => ((Helpers.Spells.GetAuraTimeLeft(StyxWoW.Me.CurrentTarget, "Rupture") < 1 && 
                                                                 Helpers.Rogue.mCurrentEnergy <= 100) ||
                                                                 !Helpers.Spells.IsAuraActive(StyxWoW.Me.CurrentTarget, "Rupture")) &&
                                                                 StyxWoW.Me.ComboPoints >= 1),

                


                Helpers.Spells.Cast("Fan of Knives", ret => Helpers.Rogue.IsAoeUsable() && 
                                                            Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 6),
		new Decorator(ret => Helpers.Rogue.IsCooldownsUsable() &&
                                     Helpers.Spells.IsAuraActive(StyxWoW.Me, "Slice and Dice") &&
                                     Helpers.Spells.IsAuraActive(StyxWoW.Me.CurrentTarget, "Rupture"),
                              new PrioritySelector(

                		Helpers.Spells.CastCooldown("Vendetta"),

                		Helpers.Spells.CastSelf("Cold Blood", ret => !StyxWoW.Me.HasAura("Cold Blood") && StyxWoW.Me.ComboPoints == 5 &&
                                                                     Helpers.Rogue.mCurrentEnergy >= 60))),


                Helpers.Spells.Cast("Envenom", ret => Helpers.Aura.DeadlyPoison &&
                                                       (((Helpers.Rogue.mCurrentEnergy >= 90 && StyxWoW.Me.ComboPoints >= 4 && StyxWoW.Me.CurrentTarget.HealthPercent >= 35) ||
                                                       (Helpers.Rogue.mCurrentEnergy >= 90 && StyxWoW.Me.ComboPoints == 5 && StyxWoW.Me.CurrentTarget.HealthPercent < 35) ||
                                                       (Helpers.Spells.GetAuraTimeLeft(StyxWoW.Me, "Slice and Dice") <= 3 && StyxWoW.Me.ComboPoints >= 1)) &&
                                                       (!Helpers.Spells.IsAuraActive(StyxWoW.Me, "Envenom") || Helpers.Rogue.mCurrentEnergy > 100))),

                

                Helpers.Spells.Cast("Mutilate", ret => !Helpers.Spells.IsAuraActive(StyxWoW.Me.CurrentTarget, "Rupture") ||
                                                       (StyxWoW.Me.ComboPoints < 4 && (Helpers.Rogue.mCurrentEnergy >= 90 ||
                                                       Helpers.Spells.IsAuraActive(StyxWoW.Me, "Envenom")))),

		Helpers.Spells.CastFocus("Tricks of the Trade", ret => !Helpers.Spells.IsAuraActive(StyxWoW.Me, "Tricks of the Trade") &&
                                                                       Helpers.Rogue.mCurrentEnergy < 90)

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
