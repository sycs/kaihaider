//////////////////////////////////////////////////
//                 Mode.cs                      //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from PallyRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Linq;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace PallyRaidBT.Settings
{
    class Mode
    {
        static public Helpers.Enumeration.CooldownUse mCooldownUse { get; set; }
        static public Helpers.Enumeration.LocationContext mLocationSettings { get; set; }

        static public bool mOverrideContext { get; set; }

        static public bool mUseMovement { get; set; }
        static public bool mUseCooldowns { get; set; }
        static public bool mUseAoe { get; set; }
        static public bool mUseCombat { get; set; }
        static public bool mForceBehind { get; set; }

        static Mode()
        {

            mCooldownUse = Helpers.Enumeration.CooldownUse.OnlyOnBosses;

            mOverrideContext = false;

            mUseMovement = true;
            mUseCooldowns = true;
            mUseAoe = true;
            mUseCombat = true;
            mForceBehind = false;
        }

        
    }
}
