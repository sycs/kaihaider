//////////////////////////////////////////////////
//           Level/Assassination.cs             //
//      Part of RogueRaidBT by kaihaider        //
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

namespace RogueRaidBT.Composites.Context.Level
{
    static class Assassination
    {
        static public Composite BuildCombatBehavior()
        {
            return new PrioritySelector(
                Helpers.Target.EnsureValidTarget(),

                new Decorator(ret => Helpers.Rogue.mHP <= 15 && Helpers.Spells.CanCast("Vanish"),
                    new Sequence(
                        Helpers.Spells.CastSelf("Vanish"),
                        new WaitContinue(2, ret => false, new ActionAlwaysSucceed())
                    )
                ),

                Helpers.Spells.ToggleAutoAttack(),

                Helpers.Specials.UseSpecialAbilities(),

                Helpers.Spells.CastSelf("Evasion",          ret => Helpers.Rogue.mHP <= 35),
                Helpers.Spells.CastSelf("Cloak of Shadows", ret => Helpers.Rogue.IsCloakUsable()),

                Helpers.Spells.Cast("Redirect",             ret => Helpers.Rogue.mComboPoints < Helpers.Rogue.mRawComboPoints),

                Helpers.Spells.CastCooldown("Vendetta",     ret => Helpers.Rogue.IsCooldownsUsable()),
                Helpers.Spells.CastSelf("Cold Blood",       ret => Helpers.Aura.ColdBlood && Helpers.Rogue.mComboPoints >= 4 && 
                                                                   Helpers.Rogue.mCurrentEnergy >= 60),

                Helpers.Spells.CastSelf("Recuperate",   ret => !Helpers.Aura.Recuperate && Helpers.Rogue.mComboPoints >= 3 && 
                                                               Helpers.Rogue.mHP <= 80),

                new Decorator(ret => Helpers.Rogue.IsInterruptUsable(),
                    new Sequence(
                        Helpers.Spells.Cast("Kick"),
                        new WaitContinue(TimeSpan.FromSeconds(0.5), ret => false, new ActionAlwaysSucceed())
                    )
                ),

                Helpers.Spells.Cast("Kidney Shot",  ret => Helpers.Rogue.mComboPoints >= 3 && (Helpers.Rogue.mHP <= 75 || 
                                                           (Helpers.Rogue.IsInterruptUsable() && Helpers.Spells.GetSpellCooldown("Kick") > 0))),
                Helpers.Spells.Cast("Rupture",      ret => Helpers.Rogue.mComboPoints >= 3 && StyxWoW.Me.CurrentTarget.HealthPercent >= 75 &&
                                                           !Helpers.Spells.IsAuraActive(StyxWoW.Me.CurrentTarget, "Rupture")),
                Helpers.Spells.Cast("Envenom",      ret => Helpers.Rogue.mComboPoints >= 4),
                Helpers.Spells.Cast("Eviscerate",   ret => Helpers.Rogue.mComboPoints >= 4),

                Helpers.Spells.Cast("Mutilate", ret => Helpers.Rogue.ReleaseSpamLock())
            );
        }

        static public Composite BuildPullBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret => StyxWoW.Me.Mounted, 
                    new Action(ret => Lua.DoString("Dismount()"))
                ),

                Helpers.Spells.CastSelf("Stealth", ret => !StyxWoW.Me.HasAura("Stealth")),

                Helpers.Spells.Cast("Cheap Shot",  ret => StyxWoW.Me.HasAura("Stealth")),
                Helpers.Spells.Cast("Mutilate")
            );
        }

        static public Composite BuildBuffBehavior()
        {
            return new Decorator(ret => !StyxWoW.Me.Mounted,
                new PrioritySelector(
                    Helpers.Spells.CastSelf("Recuperate", ret => !Helpers.Spells.IsAuraActive(StyxWoW.Me, "Recuperate") &&
                                                                     Helpers.Rogue.mRawComboPoints >= 1 && Helpers.Rogue.CheckSpamLock()),
                    Helpers.Spells.CastSelf("Slice and Dice", ret => !Helpers.Spells.IsAuraActive(StyxWoW.Me, "Slice and Dice") &&
                                                                     Helpers.Rogue.mRawComboPoints >= 1 && Helpers.Rogue.CheckSpamLock())
                )
            );
        }
    }
}
