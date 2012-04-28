//////////////////////////////////////////////////
//               Raid/Protection.cs            //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////
using System;
using CommonBehaviors.Actions;
using Styx;
using Styx.WoWInternals;
using TreeSharp;
using Action = TreeSharp.Action;

namespace PallyRaidBT.Composites.Context.Raid
{
    class Protection
    {
        static public Composite BuildCombatBehavior()
        {
            return new FlPrioritySelector(

                Helpers.Spells.ToggleAutoAttack(),

                new Decorator(ret => Helpers.Pally.IsInterruptUsable() && StyxWoW.Me.CurrentTarget.IsWithinMeleeRange,
                    new Sequence(
                        Helpers.Spells.Cast("Rebuke"),
                        new WaitContinue(TimeSpan.FromSeconds(0.5), ret => false, new ActionAlwaysSucceed())
                    )
                ),
                
                Helpers.Spells.Cast("Word of Glory", ret => (StyxWoW.Me.CurrentHolyPower == 3 && StyxWoW.Me.HealthPercent<75)),
                Helpers.Spells.CastCooldown("Divine Plea", ret => StyxWoW.Me.ManaPercent < 90),


                new Decorator(ret => Helpers.Pally.IsCooldownsUsable(),
                              new PrioritySelector(

                                  Helpers.Spells.CastCooldown("Holy Shield", ret => !Helpers.Spells.IsAuraActive(StyxWoW.Me, "Divine Protection")),

                                  Helpers.Spells.CastCooldown("Divine Protection", ret => Helpers.Spells.GetSpellCooldown("Holy Shield") > 10),

                                  Helpers.Spells.CastCooldown("Avenging Wrath"),


                                  Helpers.Specials.UseSpecialAbilities(ret => Helpers.Spells.IsAuraActive(StyxWoW.Me, "Avenging Wrath"))
                                  )
                    ),


                Helpers.Spells.Cast("Avenger's Shield", ret => StyxWoW.Me.CurrentHolyPower < 3 && Helpers.Spells.IsAuraActive(StyxWoW.Me, "Grand Crusader") && StyxWoW.Me.CurrentTarget.Distance <= 30),
                Helpers.Spells.Cast("Shield of the Righteous", ret => StyxWoW.Me.CurrentHolyPower == 3 && Helpers.Spells.IsAuraActive(StyxWoW.Me, "Sacred Duty") && StyxWoW.Me.CurrentTarget.IsWithinMeleeRange),


                new Decorator(ret => Helpers.Pally.ShouldAoe(2),
                    new PrioritySelector(
                        Helpers.Spells.Cast("Inquisition", ret => StyxWoW.Me.CurrentHolyPower == 3 && Helpers.Pally.ShouldAoe(3)),
                        Helpers.Spells.CastCooldown("Hammer of the Righteous", ret => StyxWoW.Me.CurrentTarget.IsWithinMeleeRange),
                        Helpers.Spells.CastCooldown("Consecration", ret=> StyxWoW.Me.ManaPercent > 40 && Helpers.Pally.ShouldAoe(3) && StyxWoW.Me.CurrentTarget.Distance<=10),
                        Helpers.Spells.CastCooldown("Holy Wrath", ret => StyxWoW.Me.ManaPercent > 30 && StyxWoW.Me.CurrentTarget.Distance <= 10),
                        Helpers.Spells.CastCooldown("Avenger's Shield", ret => StyxWoW.Me.CurrentTarget.Distance <= 30),
                        Helpers.Spells.Cast("Shield of the Righteous", ret => StyxWoW.Me.CurrentHolyPower == 3 && StyxWoW.Me.CurrentTarget.IsWithinMeleeRange),
                        Helpers.Spells.CastCooldown("Judgement", ret => Helpers.Spells.HasSeal() && StyxWoW.Me.CurrentTarget.Distance <= 30)
                    )
                ),

                Helpers.Spells.CastCooldown("Crusader Strike", ret=> StyxWoW.Me.CurrentTarget.IsWithinMeleeRange),
                Helpers.Spells.CastCooldown("Judgement", ret => Helpers.Spells.HasSeal() && StyxWoW.Me.CurrentTarget.Distance <= 30),
                Helpers.Spells.CastCooldown("Hammer of Wrath", ret => StyxWoW.Me.CurrentTarget.HealthPercent <= 20),
                Helpers.Spells.CastCooldown("Avenger's Shield", ret => StyxWoW.Me.CurrentTarget.Distance <= 30),
                Helpers.Spells.CastCooldown("Holy Wrath", ret => StyxWoW.Me.ManaPercent > 30 && StyxWoW.Me.CurrentTarget.Distance <= 10),
		Helpers.Spells.Cast("Shield of the Righteous", ret => StyxWoW.Me.CurrentHolyPower == 3 && StyxWoW.Me.CurrentTarget.IsWithinMeleeRange)
                        
                          
                
                );
        }

        static public Composite BuildPullBehavior()
        {
            return new PrioritySelector(
                Helpers.Spells.Cast("Seal of Truth", ret => !Helpers.Spells.HasSeal()),
                Helpers.Spells.Cast("Devotion Aura", ret => !Helpers.Spells.IsAuraActive(StyxWoW.Me, "Devotion Aura")),
                Helpers.Spells.CastCooldown("Judgement", ret => Helpers.Spells.HasSeal()&& StyxWoW.Me.CurrentTarget.Distance <= 30),
                Helpers.Spells.CastCooldown("Avenger's Shield", ret => StyxWoW.Me.CurrentTarget.Distance <= 30)
                );
        }

        static public Composite BuildBuffBehavior()
        {
            return new PrioritySelector(
                Helpers.Spells.Cast("Devotion Aura", ret => !StyxWoW.Me.HasAura("Devotion Aura")),//465)),

                Helpers.Spells.Cast("Righteous Fury", ret => !StyxWoW.Me.HasAura("Righteous Fury")),// GetAuraByName(string) 25780)),
                Helpers.Spells.Cast("Seal of Truth", ret => !Helpers.Spells.HasSeal())
                );
        }
    }
}
