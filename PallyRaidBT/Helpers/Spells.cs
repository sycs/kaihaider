﻿//////////////////////////////////////////////////
//               Helpers/Spells.cs              //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Linq;
using Styx;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace PallyRaidBT.Helpers
{
    public delegate WoWUnit WoWUnitDelegate(object context);

    static class Spells
    {
        static public Composite Cast(int spellId)
        {
            return Cast(spellId, ret => true, Color.LightBlue, ret => StyxWoW.Me.CurrentTarget);
        }

        static public Composite Cast(int spellId, CanRunDecoratorDelegate cond) 
        {
            return Cast(spellId, cond, Color.LightBlue, ret => StyxWoW.Me.CurrentTarget);
        }

        static public Composite Cast(string spellName)
        {
            return Cast(spellName, ret => true, Color.LightBlue, ret => StyxWoW.Me.CurrentTarget);
        }

        static public Composite Cast(string spellName, CanRunDecoratorDelegate cond)
        {
            return Cast(spellName, cond, Color.LightBlue, ret => StyxWoW.Me.CurrentTarget);
        }

        static public Composite CastFocus(int spellId)
        {
            return Cast(spellId, ret => Focus.mFocusTarget != null, Color.Yellow, ret => Focus.mFocusTarget);
        }

        static public Composite CastFocus(int spellId, CanRunDecoratorDelegate cond)
        {
            return Cast(spellId, ret => Focus.mFocusTarget != null && cond(ret), Color.Yellow, ret => Focus.mFocusTarget);
        }

        static public Composite CastFocus(string spellName)
        {
            return Cast(spellName, ret => Focus.mFocusTarget != null, Color.Yellow, ret => Focus.mFocusTarget);
        }

        static public Composite CastFocus(string spellName, CanRunDecoratorDelegate cond)
        {
            return Cast(spellName, ret => Focus.mFocusTarget != null && cond(ret), Color.Yellow, ret => Focus.mFocusTarget);
        }

        static public Composite CastCooldown(int spellId)
        {
            return Cast(spellId, ret => true, Color.Red, ret => StyxWoW.Me.CurrentTarget);
        }

        static public Composite CastCooldown(int spellId, CanRunDecoratorDelegate cond)
        {
            return Cast(spellId, cond, Color.Red, ret => StyxWoW.Me.CurrentTarget);
        }

        static public Composite CastCooldown(string spellName)
        {
            return Cast(spellName, ret => true, Color.Red, ret => StyxWoW.Me.CurrentTarget);
        }

        static public Composite CastCooldown(string spellName, CanRunDecoratorDelegate cond)
        {
            return Cast(spellName, cond, Color.Red, ret => StyxWoW.Me.CurrentTarget);
        }

        static public Composite CastSelf(int spellId)
        {
            return Cast(spellId, ret => true, Color.DarkSalmon, ret => StyxWoW.Me);
        }

        static public Composite CastSelf(int spellId, CanRunDecoratorDelegate cond)
        {
            return Cast(spellId, cond, Color.DarkSalmon, ret => StyxWoW.Me);
        }

        static public Composite CastSelf(string spellName)
        {
            return Cast(spellName, ret => true, Color.DarkSalmon, ret => StyxWoW.Me);
        }

        static public Composite CastSelf(string spellName, CanRunDecoratorDelegate cond)
        {
            return Cast(spellName, cond, Color.DarkSalmon, ret => StyxWoW.Me);
        }

        static public Composite ToggleAutoAttack()
        { 
            return ToggleAutoAttack(ret => true);
        }

        static public Composite ToggleAutoAttack(CanRunDecoratorDelegate cond)
        {
            return new Decorator(ret => cond(ret) && !StyxWoW.Me.IsAutoAttacking,
                new Action(ret => 
                    {
                        StyxWoW.Me.ToggleAttack();
                        Logging.Write(Color.Orange, "Auto-attack");
                    }
                )
            );
        }

        

        static public bool HasSeal()
        {
            if(Helpers.Spells.IsAuraActive(StyxWoW.Me, "Seal of Justice") || Helpers.Spells.IsAuraActive(StyxWoW.Me, "Seal of Righteousness") || Helpers.Spells.IsAuraActive(StyxWoW.Me, "Seal of Truth"))
            {
                return true;
            }
            else
            {
                return false;
            }
             
        }

        static public bool HasSpell(string spellName)
        {
            return SpellManager.HasSpell(spellName);
        }

        static public bool CanCast(int spellId)
        {
            return SpellManager.HasSpell(spellId) && GetSpellCooldown(spellId) <= 0.25 && StyxWoW.Me.CurrentMana >= SpellManager.Spells[WoWSpell.FromId(spellId).Name].PowerCost;
        }

        static public bool CanCast(string spellName)
        {
            return SpellManager.HasSpell(spellName) && GetSpellCooldown(spellName) <= 0.25 && StyxWoW.Me.CurrentMana >= SpellManager.Spells[spellName].PowerCost;
        }

        static public bool IsAuraActive(WoWUnit auraTarget, int auraId)
        {
            return GetAuraTimeLeft(auraTarget, auraId, StyxWoW.Me.Guid) > 0;
        }

        static public bool IsAuraActive(WoWUnit auraTarget, int auraId, UInt64 creatorGuid)
        {
            return GetAuraTimeLeft(auraTarget, auraId, creatorGuid) > 0;
        }

        static public bool IsAuraActive(WoWUnit auraTarget, string auraName)
        {
            return GetAuraTimeLeft(auraTarget, auraName, StyxWoW.Me.Guid) > 0;
        }

        static public bool IsAuraActive(WoWUnit auraTarget, string auraName, UInt64 creatorGuid)
        {
            return GetAuraTimeLeft(auraTarget, auraName, creatorGuid) > 0;
        }

        static public double GetAuraTimeLeft(WoWUnit auraTarget, int auraId)
        {
            return GetAuraTimeLeft(auraTarget, auraId, StyxWoW.Me.Guid);
        }

        static public double GetAuraTimeLeft(WoWUnit auraTarget, int auraId, UInt64 creatorGuid)
        {
            WoWAuraCollection unitAuras = auraTarget.GetAllAuras();

            return (from aura in unitAuras 
                    where aura.SpellId == auraId && aura.CreatorGuid == creatorGuid
                    select aura.TimeLeft.TotalSeconds).FirstOrDefault();
        }

        static public double GetAuraTimeLeft(WoWUnit auraTarget, string auraName)
        {
            return GetAuraTimeLeft(auraTarget, auraName, StyxWoW.Me.Guid);
        }

        static public double GetAuraTimeLeft(WoWUnit auraTarget, string auraName, UInt64 creatorGuid)
        {
            WoWAuraCollection unitAuras = auraTarget.GetAllAuras();

            return (from aura in unitAuras 
                    where aura.Name == auraName && aura.CreatorGuid == creatorGuid 
                    select aura.TimeLeft.TotalSeconds).FirstOrDefault();
        }

        static public double GetSpellCooldown(int spellId)
        {
            return SpellManager.Spells[WoWSpell.FromId(spellId).Name].CooldownTimeLeft.TotalSeconds;
        }

        static public double GetSpellCooldown(string spellName)
        {
            return SpellManager.Spells[spellName].CooldownTimeLeft.TotalSeconds;
        }

        static private Composite Cast(int spellId, CanRunDecoratorDelegate cond, Color color, WoWUnitDelegate target) 
        {
            return new Decorator(ret => target(ret) != null && cond(ret) && CanCast(spellId),
                new Action(ret =>
                    {
                        SpellManager.Cast(spellId, target(ret));
                        Logging.Write(color, "[" + StyxWoW.Me.CurrentHolyPower + " HP] " + WoWSpell.FromId(spellId).Name);
                    }
                )
            );
        }

        static private Composite Cast(string spellName, CanRunDecoratorDelegate cond, Color color, WoWUnitDelegate target)
        {
            return new Decorator(ret => target(ret) != null && cond(ret) && CanCast(spellName),
                new Action(ret =>
                    {
                        SpellManager.Cast(spellName, target(ret));
                        Logging.Write(color, "[" + StyxWoW.Me.CurrentHolyPower + " HP] " + spellName);
                    }
                )
            );
        }
    }
}
