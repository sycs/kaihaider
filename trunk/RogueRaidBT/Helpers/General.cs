//////////////////////////////////////////////////
//                General.cs                    //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Diagnostics;
using Styx;
using Styx.Helpers;
using Styx.Logic.Pathing;

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
            using (new FrameLock())
            {
               

                    
                    Target.Pulse();
                    Area.Pulse();
                    Rogue.Pulse();
                    Focus.Pulse();
                    Specials.Pulse();
                    Aura.Pulse();

                    Movement.Pulse();
                    //Target.EnsureValidTarget();
                
            }
        }
        
    }
}
