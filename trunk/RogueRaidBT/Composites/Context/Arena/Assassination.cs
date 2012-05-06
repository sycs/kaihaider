//////////////////////////////////////////////////
//            Raid/Assassination.cs             //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Linq;
using CommonBehaviors.Actions;
using Styx;
using TreeSharp;

namespace RogueRaidBT.Composites.Context.Arena 
{
    static class Assassination

    {
        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(
                Helpers.Spells.ToggleAutoAttack(),

        Helpers.Spells.Cast("Recuperate", ret => (StyxWoW.Me.CurrentTarget.CastingSpellId == 109033 || StyxWoW.Me.CurrentTarget.CastingSpellId ==109034) &&
            Helpers.Rogue.mCurrentEnergy >= 30 && Helpers.Rogue.mHP < 95 && 
							Helpers.Spells.GetAuraTimeLeft(StyxWoW.Me, "Recuperate") < 2 && StyxWoW.Me.ComboPoints > 2),


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
