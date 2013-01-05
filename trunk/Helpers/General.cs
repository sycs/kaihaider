//////////////////////////////////////////////////
//                General.cs                    //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Diagnostics;
using Styx;
using Styx.MemoryManagement;
using Styx.Helpers;
using Styx.Pathing;

namespace RogueRaidBT.Helpers
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
            
               

                    if(StyxWoW.Me != null)
                    {
                        Target.Pulse();
                        Area.Pulse();
                        Rogue.Pulse();
                        Focus.Pulse();
                        Specials.Pulse();
                        Aura.Pulse();
                        if (Movement.IsPVPSuiteEnabled) Settings.Mode.mUseMovement = false;

                    }
                    

                    //Target.EnsureValidTarget();
                
            
        }

        
        
    }
}
