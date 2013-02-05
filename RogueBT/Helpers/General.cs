//////////////////////////////////////////////////
//                General.cs                    //
//        Part of RogueBT by kaihaider          //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Diagnostics;
using Styx;
using Styx.MemoryManagement;
using Styx.Helpers;
using Styx.Pathing;

namespace RogueBT.Helpers
{
    static class General
    {
        static public Stopwatch mTimer = new Stopwatch();

        static General()
        {
            mTimer.Start();
        }

        static public void UpdateHelpers()
        {

            if (StyxWoW.Me != null && StyxWoW.IsInGame)
            {
                        Area.Pulse();
                        Target.Pulse();
                        Rogue.Pulse();
                        Focus.Pulse();
                        Specials.Pulse();
                        Aura.Pulse();
                        

                    }
        }       
    }
}
