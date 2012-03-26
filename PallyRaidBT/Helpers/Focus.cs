//////////////////////////////////////////////////
//               Helpers/Focus.cs               //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using Styx;
using Styx.WoWInternals.WoWObjects;

namespace PallyRaidBT.Helpers
{
    static class Focus
    {
        static public WoWUnit mFocusTarget { get; private set; }

        static public void Pulse()
        {
            mFocusTarget = GetFocusTarget();
        }

        static private WoWUnit GetFocusTarget()
        {
            WoWUnit curFocus = StyxWoW.Me.FocusedUnit;

            if (curFocus != null && curFocus.InLineOfSpellSight && curFocus.IsAlive)
            {
                return curFocus;
            }

            return null;
        }
    }
}
