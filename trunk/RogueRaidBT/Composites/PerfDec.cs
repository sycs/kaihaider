//////////////////////////////////////////////////
//                PerfDec.cs                    //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using Styx;
using TreeSharp;
using Styx.Helpers;
using System.Drawing;

namespace RogueRaidBT.Composites
{
    class PerfDec : Decorator
    {
        public PerfDec(Composite child)
            : base(child)
        {
        }

        public override RunStatus Tick(object context)
        {
            // Wraps the entire tree in a framelock and ticks the tree. 
            // Responsible for debug text.
            
                using (new FrameLock())
                {
                    //base.Start(context);
                    Helpers.General.mTimer.Reset();
                    Helpers.General.mTimer.Start();

                    Logging.WriteDebug(Color.Orange, "START TICK");


                    RunStatus tick = base.Tick(context);

                    if (Helpers.General.mTimer.ElapsedMilliseconds!=0) Logging.WriteDebug(Color.Orange, "END TICK -> {0} ms", Helpers.General.mTimer.ElapsedMilliseconds);
                    //base.Stop(context);
                    return tick;
                }
            
        }
    }
}