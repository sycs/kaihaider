//////////////////////////////////////////////////
//                 Focus.cs                     //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using Styx;
using Styx.WoWInternals.WoWObjects;

namespace RogueBT.Helpers
{
    static class Focus
    {
        static public WoWUnit mFocusTarget { get; private set; }
        static public WoWUnit rawFocusTarget { get; private set; }

        static public void Pulse()
        {
            mFocusTarget = GetFocusTarget();
        }

        static private WoWUnit GetFocusTarget()
        {
            WoWUnit curFocus = Helpers.Rogue.me.FocusedUnit;
            rawFocusTarget = null;
            if (curFocus != null && curFocus.InLineOfSpellSight && curFocus.IsAlive ) rawFocusTarget = curFocus;

            if (curFocus != null && curFocus.InLineOfSpellSight && curFocus.IsAlive && curFocus.Guid != Helpers.Rogue.me.Guid && curFocus.ToPlayer() != null && curFocus.ToPlayer().IsInMyPartyOrRaid)
            {
                return curFocus;
            }

            return null;
        }
    }
}
