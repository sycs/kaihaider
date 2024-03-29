﻿//////////////////////////////////////////////////
//                 Spells.cs                    //
//        Part of RogueBT by kaihaider          //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System;
using System.Windows.Media;
using System.Linq;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;
using Styx.WoWInternals;

namespace RogueBT.Helpers
{
    static class Spells
    {
        static public bool FindSpell(int spellId)
        {
            Styx.CommonBot.SpellFindResults TempSpellFind;
            return Styx.CommonBot.SpellManager.FindSpell(spellId, out TempSpellFind);
        }

        static public Composite Cast(int spellId)
        {
            return Cast(spellId, ret => true, ret => Helpers.Rogue.mTarget);
        }


        static public Composite Cast(int spellId, CanRunDecoratorDelegate cond) 
        {
            return Cast(spellId, cond, ret => Helpers.Rogue.mTarget);
        }

        static public Composite Cast(string spellName)
        {
            return Cast(spellName, ret => true, ret => Helpers.Rogue.mTarget);
        }

        static public Composite Cast(string spellName, CanRunDecoratorDelegate cond)
        {
            return Cast(spellName, cond, ret => Helpers.Rogue.mTarget);
        }

        static public Composite CastFocusRaw(int spellId)
        {
            return Cast(spellId, ret => true, ret => Focus.rawFocusTarget);
        }

        static public Composite CastFocusRaw(int spellId, CanRunDecoratorDelegate cond)
        {
            return Cast(spellId, cond,  ret => Focus.rawFocusTarget);
        }

        static public Composite CastFocusRaw(string spellName)
        {
            return Cast(spellName, ret => true, ret => Focus.rawFocusTarget);
        }

        static public Composite CastFocusRaw(string spellName, CanRunDecoratorDelegate cond)
        {
            return Cast(spellName, cond,ret => Focus.rawFocusTarget);
        }

        static public Composite CastFocus(int spellId)
        {
            return Cast(spellId, ret => true, ret => Focus.mFocusTarget);
        }

        static public Composite CastFocus(int spellId, CanRunDecoratorDelegate cond)
        {
            return Cast(spellId, cond, ret => Focus.mFocusTarget);
        }

        static public Composite CastFocus(string spellName)
        {
            return Cast(spellName, ret => true,  ret => Focus.mFocusTarget);
        }

        static public Composite CastFocus(string spellName, CanRunDecoratorDelegate cond)
        {
            return Cast(spellName, cond,  ret => Focus.mFocusTarget);
        }

        static public Composite CastCooldown(int spellId)
        {
            return Cast(spellId, ret => true, ret => Helpers.Rogue.mTarget);
        }

        static public Composite CastCooldown(int spellId, CanRunDecoratorDelegate cond)
        {
            return Cast(spellId, cond, ret => Helpers.Rogue.mTarget);
        }

        static public Composite CastCooldown(string spellName)
        {
            return Cast(spellName, ret => true, ret => Helpers.Rogue.mTarget);
        }

        static public Composite CastCooldown(string spellName, CanRunDecoratorDelegate cond)
        {
            return Cast(spellName, cond, ret => Helpers.Rogue.mTarget);
        }

        static public Composite CastSelf(int spellId)
        {
            return Cast(spellId, ret => true, ret => Helpers.Rogue.me);
        }

        static public Composite CastSelf(int spellId, CanRunDecoratorDelegate cond)
        {
            return Cast(spellId, cond, ret => Helpers.Rogue.me);
        }

        static public Composite CastSelf(string spellName)
        {
            return Cast(spellName, ret => true,  ret => Helpers.Rogue.me);
        }

        static public Composite CastSelf(string spellName, CanRunDecoratorDelegate cond)
        {
            return Cast(spellName, cond, ret => Helpers.Rogue.me);
        }

        static public Composite ToggleAutoAttack()
        {
            return new Decorator(ret => Helpers.Rogue.mTarget != null && !Helpers.Aura.Stealth && !Helpers.Aura.Vanish &&
                !Helpers.Aura.IsTargetDisoriented && !Helpers.Aura.IsTargetSapped && !Helpers.Rogue.me.IsAutoAttacking,
                 new Sequence(
                    new Action(ret => 
                    {
                        Helpers.Rogue.me.ToggleAttack();
                        Logging.Write(LogLevel.Normal, "Auto-attack");
                    }),
                    new CommonBehaviors.Actions.ActionAlwaysFail()));
        }

        static public bool CanCast(int spellId)
        {
            return CanCast(WoWSpell.FromId(spellId).Name);
        }

        static public bool CanCast(string spellName)
        {
           // if (spellName.Equals("Hemorrhage")) return SpellManager.GlobalCooldownLeft.TotalSeconds < 0.5 && Rogue.mCurrentEnergy >= 30 - 5;
          //  if (spellName.Equals("Envenom")) return SpellManager.GlobalCooldownLeft.TotalSeconds < 0.5 && Rogue.mCurrentEnergy >= 35 - 5;
            return SpellManager.HasSpell(spellName) && ((GetSpellCooldown(spellName) < 0.2) || (SpellManager.GlobalCooldown && GetSpellCooldown(spellName) < 0.5)) && Rogue.mCurrentEnergy >= SpellManager.Spells[spellName].PowerCost - 5;
        }

        static public bool IsAuraActive(WoWUnit auraTarget, int auraId)
        {
            return GetAuraTimeLeft(auraTarget, auraId, Helpers.Rogue.me.Guid) > 0;
        }

        static public bool IsAuraActive(WoWUnit auraTarget, int auraId, UInt64 creatorGuid)
        {
            return GetAuraTimeLeft(auraTarget, auraId, creatorGuid) > 0;
        }

        static public bool IsAuraActive(WoWUnit auraTarget, string auraName)
        {
            return GetAuraTimeLeft(auraTarget, auraName, Helpers.Rogue.me.Guid) > 0;
        }

        static public bool IsAuraActive(WoWUnit auraTarget, string auraName, UInt64 creatorGuid)
        {
            return GetAuraTimeLeft(auraTarget, auraName, creatorGuid) > 0;
        }

        static public double GetAuraTimeLeft(WoWUnit auraTarget, int auraId)
        {
            return GetAuraTimeLeft(auraTarget, auraId, Helpers.Rogue.me.Guid);
        }

        static public double GetAuraTimeLeft(WoWUnit auraTarget, int auraId, UInt64 creatorGuid)
        {
		if( auraTarget == null) return 0;
            return (from aura in auraTarget.GetAllAuras() 
                    where aura.SpellId == auraId && aura.CreatorGuid == creatorGuid
                    select aura.TimeLeft.TotalSeconds).FirstOrDefault();
        }

        static public double GetAuraTimeLeft(WoWUnit auraTarget, string auraName)
        {
            return GetAuraTimeLeft(auraTarget, auraName, Helpers.Rogue.me.Guid);
        }

        static public double GetAuraTimeLeft(WoWUnit auraTarget, string auraName, UInt64 creatorGuid)
        {
		if( auraTarget == null) return 0;
            return (from aura in auraTarget.GetAllAuras()  
                    where aura.Name == auraName && aura.CreatorGuid == creatorGuid 
                    select aura.TimeLeft.TotalSeconds).FirstOrDefault();
        }

        static public double GetSpellCooldown(int spellId)
        {
            return GetSpellCooldown(WoWSpell.FromId(spellId).Name);
        }

        static public double GetSpellCooldown(string spellName)
        {
            return SpellManager.HasSpell(spellName) ? SpellManager.Spells[spellName].CooldownTimeLeft.TotalSeconds : 0;
        }

        static public Composite Cast(int spellId, CanRunDecoratorDelegate cond, WoWUnitDelegate target) 
        {
            return new Decorator(ret => target != null && cond(ret) && CanCast(spellId),
                new Action(ret =>
                    {
                        SpellManager.Cast(spellId, target(ret));
                        Logging.Write(LogLevel.Diagnostic, Colors.White, "" + WoWSpell.FromId(spellId).Name);
                        string temp;
                        if (target(ret).IsPlayer)
                            temp = "Casting " + WoWSpell.FromId(spellId).Name + " on Player at " +  
                                             Math.Round(target(ret).HealthPercent, 0) + " with " + Helpers.Rogue.me.ComboPoints + "CP and " + 
                                             Rogue.mCurrentEnergy + " energy";
                        else
                            temp = "Casting " + WoWSpell.FromId(spellId).Name + " on " + target(ret).Name + " at " +  
                                             Math.Round(target(ret).HealthPercent, 0) + " with " + Helpers.Rogue.me.ComboPoints + "CP and " + 
                                             Rogue.mCurrentEnergy + " energy";

                        Logging.Write(LogLevel.Normal, temp );
                    }
                )
            );
        }

        static public Composite CastFail(string spellName, CanRunDecoratorDelegate cond, WoWUnitDelegate target)
        {
            if (spellName.Equals("Hemorrhage")) spellName = "Sinister Strike";
            if (spellName.Equals("Dispatch")) spellName = "Sinister Strike";
            if (spellName.Equals("Envenom")) spellName = "Eviscerate";
            return new Decorator(ret => target(ret) != null && cond(ret) && SpellManager.HasSpell(spellName),
                new Action(ret =>
                {
                    SpellManager.Cast(spellName, target(ret));
                    if (target(ret).IsPlayer)
                        Logging.Write(LogLevel.Normal, "Attempting " + spellName + " on Player at " +
                                             Math.Round(target(ret).HealthPercent, 0) + "% with " + Helpers.Rogue.mComboPoints + "CP and " +
                                             Rogue.mCurrentEnergy + " energy");
                    else
                        Logging.Write(LogLevel.Normal, "Attempting " + spellName + " on " + target(ret).Name + " at " +
                                         Math.Round(target(ret).HealthPercent, 0) + "% with " + Helpers.Rogue.mComboPoints + "CP and " +
                                         Rogue.mCurrentEnergy + " energy");
                    return RunStatus.Failure;
                })
            );
        }

        static public Composite Cast(string spellName, CanRunDecoratorDelegate cond,  WoWUnitDelegate target)
        {
                if (spellName.Equals("Hemorrhage")) spellName = "Sinister Strike";
                if (spellName.Equals("Dispatch")) spellName = "Sinister Strike";
                if (spellName.Equals("Envenom")) spellName = "Eviscerate"; 
            return new Decorator(ret => target(ret) != null && cond(ret) && CanCast(spellName),
                new Action(ret =>
                     {
                        SpellManager.Cast(spellName, target(ret));
                        if (target(ret).IsPlayer)
                            Logging.Write(LogLevel.Normal, "Casting " + spellName + " on Player at " +
                                                 Math.Round(target(ret).HealthPercent, 0) + "% with " + Helpers.Rogue.mComboPoints + "CP and " +
                                                 Rogue.mCurrentEnergy + " energy");
                        else 
                            Logging.Write(LogLevel.Normal , "Casting " + spellName + " on " + target(ret).Name + " at " +
                                             Math.Round(target(ret).HealthPercent, 0) + "% with " + Helpers.Rogue.mComboPoints + "CP and " +
                                             Rogue.mCurrentEnergy + " energy");
                    })
            );
        }

        /// Stolen from singular
        /// <summary>
        ///  Checks for the auras on a specified unit. Returns true if the unit has any aura with any of the mechanics in the mechanics list.
        /// </summary>
        /// <param name="unit"> The unit to check auras for. </param>
        /// <param name="mechanics"> Mechanics to be checked. </param>
        /// <returns></returns>
        public static bool HasAuraWithMechanic(this WoWUnit unit, params WoWSpellMechanic[] mechanics)
        {
            var auras = unit.GetAllAuras();
            return auras.Any(a => mechanics.Contains(a.Spell.Mechanic));
        }
        
               // Spell.WaitForCastOrChannel(),
    }
}
