//////////////////////////////////////////////////
//                 Focus.cs                     //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using Styx;
using Styx.WoWInternals.WoWObjects;

namespace RogueRaidBT.Helpers
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
            WoWUnit curFocus = StyxWoW.Me.FocusedUnit;
            rawFocusTarget = null;
            if (curFocus != null && curFocus.InLineOfSpellSight && curFocus.IsAlive ) rawFocusTarget = curFocus;

            if (curFocus != null && curFocus.InLineOfSpellSight && curFocus.IsAlive && curFocus.Guid != StyxWoW.Me.Guid && curFocus.ToPlayer() != null && curFocus.ToPlayer().IsInMyPartyOrRaid)
            {
                return curFocus;
            }

            return null;
        }
    }
}
