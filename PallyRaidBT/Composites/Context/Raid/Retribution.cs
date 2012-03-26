//////////////////////////////////////////////////
//               Raid/Retribution.cs            //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////
using CommonBehaviors.Actions;
using Styx;
using TreeSharp;

namespace PallyRaidBT.Composites.Context.Raid
{
    class Retribution
    {
        static public Composite BuildCombatBehavior()
        {
            return new FlPrioritySelector(

    Helpers.Spells.ToggleAutoAttack(),
    Helpers.Spells.Cast("Inquisition", ret => Helpers.Spells.GetAuraTimeLeft(StyxWoW.Me, "Inquisition") < 4 &&
                                   (StyxWoW.Me.CurrentHolyPower == 3 || Helpers.Spells.IsAuraActive(StyxWoW.Me, "Divine Purpose"))),
    Helpers.Spells.CastCooldown("Crusader Strike", ret => StyxWoW.Me.CurrentHolyPower < 3 && !Settings.Mode.ShouldAoe(4)),
    Helpers.Spells.CastCooldown("Divine Storm", ret => StyxWoW.Me.CurrentHolyPower < 3 && Settings.Mode.ShouldAoe(4)),

    new Decorator(ret => Settings.Mode.ShouldUseCooldowns() &&
                         Helpers.Spells.IsAuraActive(StyxWoW.Me, "Inquisition"),

        new PrioritySelector(
            Helpers.Trinkets.UseTrinkets(ret => true),

            Helpers.Spells.CastCooldown("Guardian of Ancient Kings", ret =>
                        (Helpers.Spells.GetSpellCooldown("Zealotry") > 60 || Helpers.Spells.GetSpellCooldown("Zealotry") < 10)),

            Helpers.Spells.CastCooldown("Zealotry", ret =>
                                                                 (StyxWoW.Me.CurrentHolyPower == 3 ||
                                                                    Helpers.Spells.IsAuraActive(StyxWoW.Me, "Divine Purpose")) &&
                                                                 (!Helpers.Spells.HasSpell("Guardian of Ancient Kings") || Helpers.Spells.GetAuraTimeLeft(StyxWoW.Me, "Guardian of Ancient Kings") <= 20 ||
                                                                    (Helpers.Spells.GetSpellCooldown("Guardian of Ancient Kings") <= 120
                                                                    &&Helpers.Spells.GetSpellCooldown("Guardian of Ancient Kings") >= 60))),

            Helpers.Spells.CastCooldown("Avenging Wrath", ret => Helpers.Spells.IsAuraActive(StyxWoW.Me, "Zealotry"))
            )
            ),

    new Decorator(ret => Helpers.Spells.IsAuraActive(StyxWoW.Me, "Zealotry") && Helpers.Spells.GetSpellCooldown("Crusader Strike") >= 0.5,
            new PrioritySelector(

                Helpers.Spells.Cast("Templar's Verdict", ret => StyxWoW.Me.CurrentHolyPower == 3 || Helpers.Spells.IsAuraActive(StyxWoW.Me, "Divine Purpose")),
                Helpers.Spells.CastCooldown("Consecration", ret => Settings.Mode.ShouldAoe(4) && StyxWoW.Me.ManaPercent > 65),
                Helpers.Spells.CastCooldown("Hammer of Wrath", ret => StyxWoW.Me.CurrentTarget.HealthPercent < 20 ||
                                                       Helpers.Spells.IsAuraActive(StyxWoW.Me, "Avenging Wrath")),
                Helpers.Spells.Cast("Exorcism", ret => Helpers.Spells.IsAuraActive(StyxWoW.Me, "The Art of War")),
                Helpers.Spells.CastCooldown("Judgement", ret => Helpers.Spells.HasSeal()),
                Helpers.Spells.CastCooldown("Holy Wrath", ret => StyxWoW.Me.ManaPercent > 35),
                Helpers.Spells.Cast("Consecration", ret => StyxWoW.Me.ManaPercent > 65)


)),


    new Decorator(ret => !Helpers.Spells.IsAuraActive(StyxWoW.Me, "Zealotry") && Helpers.Spells.GetSpellCooldown("Crusader Strike") >= 0.5,
            new PrioritySelector(



                Helpers.Spells.CastCooldown("Consecration", ret => Settings.Mode.ShouldAoe(3) && StyxWoW.Me.ManaPercent > 65),
                Helpers.Spells.CastCooldown("Hammer of Wrath", ret => StyxWoW.Me.CurrentTarget.HealthPercent < 20 ||
                                                       Helpers.Spells.IsAuraActive(StyxWoW.Me, "Avenging Wrath")),
                Helpers.Spells.Cast("Exorcism", ret => Helpers.Spells.IsAuraActive(StyxWoW.Me, 59578)),
                Helpers.Spells.Cast("Templar's Verdict", ret => StyxWoW.Me.CurrentHolyPower == 3 || Helpers.Spells.IsAuraActive(StyxWoW.Me, "Divine Purpose")),
                Helpers.Spells.CastCooldown("Seal of Truth", ret => !Helpers.Spells.HasSeal()),
                Helpers.Spells.CastCooldown("Judgement", ret => Helpers.Spells.HasSeal()),
                Helpers.Spells.CastCooldown("Holy Wrath", ret => StyxWoW.Me.ManaPercent > 35),
                Helpers.Spells.CastCooldown("Consecration", ret => StyxWoW.Me.ManaPercent > 65),
                Helpers.Spells.CastCooldown("Divine Plea", ret => StyxWoW.Me.ManaPercent <= 90),
                Helpers.Spells.CastCooldown("Sacred Shield", ret => StyxWoW.Me.HealthPercent <= 30),

                Helpers.Spells.Cast("Templar's Verdict", ret => StyxWoW.Me.CurrentHolyPower > 0)



))




);
        }

        static public Composite BuildPullBehavior()
        {
            return new PrioritySelector(
                Helpers.Spells.Cast("Seal of Truth", ret => !Helpers.Spells.HasSeal())
                
                
            );
        }

        static public Composite BuildBuffBehavior()
        {
            return new PrioritySelector(
                Helpers.Spells.Cast("Seal of Truth", ret => !Helpers.Spells.HasSeal()),
                Helpers.Spells.Cast("Blessing of Kings", ret => Helpers.Spells.IsAuraActive(StyxWoW.Me, "Blessing of Might")
                        && !Helpers.Spells.IsAuraActive(StyxWoW.Me, "Blessing of Might", StyxWoW.Me.Guid)
                        && !Helpers.Spells.IsAuraActive(StyxWoW.Me, "Blessing of Kings")),
                Helpers.Spells.Cast("Blessing of Might", ret => !Helpers.Spells.IsAuraActive(StyxWoW.Me, "Blessing of Might"))

            );
        }
    }
}
