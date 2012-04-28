//////////////////////////////////////////////////
//               Helpers/Pally.cs               //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////
using System.Drawing;
using System.Linq;
using System;
using System.Collections.Generic;
using CommonBehaviors.Actions;
using Styx;
using Styx.Helpers;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;


namespace PallyRaidBT.Helpers
{
    static class Pally
    {

        static public Enumeration.TalentTrees mCurrentSpec { get; private set; }



        static Pally()
        {
            Lua.Events.AttachEvent("CHARACTER_POINTS_CHANGED", delegate
                {
                    Logging.Write(Color.Orange, "Your spec has been updated. Rebuilding behaviors...");
                    mCurrentSpec = GetCurrentSpecLua();
                }
            );

            Lua.Events.AttachEvent("ACTIVE_TALENT_GROUP_CHANGED", delegate
                {
                    Logging.Write(Color.Orange, "Your spec has changed. Rebuilding behaviors...");
                    mCurrentSpec = GetCurrentSpecLua();
                }
            );

            mCurrentSpec = GetCurrentSpecLua();
        }

        static public void Pulse()
        {
            
        }

        static public bool IsInterruptUsable()
        {
            return StyxWoW.Me.CurrentTarget.IsCasting && StyxWoW.Me.CurrentTarget.CurrentCastTimeLeft.TotalSeconds <= 0.5;
        }

        static public bool IsCooldownsUsable()
        {
            if (Settings.Mode.mUseCooldowns)
            {
                switch (Settings.Mode.mCooldownUse)
                {
                    case Enumeration.CooldownUse.Always:

                        return true;

                    case Enumeration.CooldownUse.ByFocus:

                        return Focus.mFocusTarget != null && Focus.mFocusTarget.Guid == StyxWoW.Me.CurrentTarget.Guid &&
                               !Focus.mFocusTarget.IsFriendly;

                    case Enumeration.CooldownUse.OnlyOnBosses:

                        return Area.IsCurTargetSpecial();
                }
            }

            return false;
        }
        static public bool IsBehindUnit(WoWUnit unit)
        {
            return Settings.Mode.mForceBehind || unit.MeIsBehind;
        }
        
        

        // Returns the index of the current active dual spec -- first or second.
        static private int GetSpecGroupLua()
        {
            return Lua.GetReturnVal<int>("return GetActiveTalentGroup(false, false)", 0);
        }


        static private Enumeration.TalentTrees GetCurrentSpecLua()
        {
            int group = GetSpecGroupLua();

            var pointsSpent = new int[3];

            for (int tab = 1; tab <= 3; tab++)
            {
                List<string> talentTabInfo = Lua.GetReturnValues("return GetTalentTabInfo(" + tab + ", false, false, " + group + ")");
                pointsSpent[tab - 1] = Convert.ToInt32(talentTabInfo[4]);
            }

            if (pointsSpent[0] > (pointsSpent[1] + pointsSpent[2]))
            {
                return Enumeration.TalentTrees.Holy;
            }

            if (pointsSpent[1] > (pointsSpent[0] + pointsSpent[2]))
            {
                return Enumeration.TalentTrees.Protection;
            }

            if (pointsSpent[2] > (pointsSpent[0] + pointsSpent[1]))
            {
                return Enumeration.TalentTrees.Retribution;
            }

            return Enumeration.TalentTrees.None;
        }

        static public bool IsAoeUsable()
        {
            return Settings.Mode.mUseAoe;
        }

        static public bool ShouldAoe(int num)
        {
            if (Settings.Mode.mUseAoe)
            {
                if (ObjectManager.GetObjectsOfType<WoWUnit>(true, false).Count(unit => unit.Distance2D <= 8 && !unit.Dead && !unit.IsFriendly && !unit.IsNonCombatPet) >= num)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
