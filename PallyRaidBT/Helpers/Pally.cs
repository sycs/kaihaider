//////////////////////////////////////////////////
//               Helpers/Pally.cs               //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace PallyRaidBT.Helpers
{
    static class Pally
    {
        
        static public SpecList mCurrentSpec { get { return GetCurrentSpecLua(); } }

        public enum SpecList
        {
            None = 0,
            Retribution,
            Holy,
            Protection
        }

        static public void Pulse()
        {
            
        }


        static public bool IsBehindUnit(WoWUnit unit)
        {
            if (Settings.Mode.mForceBehind)
            {
                return true;
            }

            return unit.MeIsBehind;
        }
        
        

        // Returns the index of the current active dual spec -- first or second.
        static private int GetSpecGroupLua()
        {
            return Lua.GetReturnVal<int>("return GetActiveTalentGroup(false, false)", 0);
        }


        static private SpecList GetCurrentSpecLua()
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
                return SpecList.Holy;
            }

            if (pointsSpent[1] > (pointsSpent[0] + pointsSpent[2])) 
            {
                return SpecList.Protection;
            }

            if (pointsSpent[2] > (pointsSpent[0] + pointsSpent[1]))
            {
                return SpecList.Retribution;
            }

            return SpecList.None;
        }
    }
}
