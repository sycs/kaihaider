//////////////////////////////////////////////////
//                 RogueBT.cs                   //
//         Part of RogueBT by kaihaider         //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System;
using System.Drawing;
using RogueBT.Composites;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.TreeSharp;


[assembly: System.Reflection.AssemblyVersion("0.3.2.1")]

namespace RogueBT
{
    class RogueRaidBT : CombatRoutine
    {

        public override string Name { get { return "RogueBT"; } }
        public override WoWClass Class { get { return WoWClass.Rogue; } }
        public override bool WantButton { get { return true; } }

        public override Composite CombatBehavior { get { return new PerfDec(Composites.Composites.BuildCombatBehavior()); } }
        public override Composite PullBehavior { get { return Composites.Composites.BuildPullBehavior(); } }
        public override Composite PreCombatBuffBehavior { get { return Composites.Composites.BuildBuffBehavior(); } }
        public override Composite RestBehavior { get { return Composites.Rest.BuildRestBehavior(); } }
       // public override Composite MoveToTargetBehavior { get { return Helpers.Movement.MoveToTarget(); } }
        

        public override void Initialize()
        {
            Logging.Write(LogLevel.Normal, "");
            Logging.Write(LogLevel.Normal, "RogueBT is now operational.");
            Logging.Write(LogLevel.Normal, "");
            Logging.Write(LogLevel.Normal, "Your feedback is appreciated. Please leave some in the forum thread at:");
            Logging.Write(LogLevel.Normal, "http://www.thebuddyforum.com/honorbuddy-forum/classes/rogue/...");
            Logging.Write(LogLevel.Normal, "");
            Logging.Write(LogLevel.Normal, "");
        }

        public override void OnButtonPress()
        {
            var configUi = new UI.Config();
            configUi.Show();
        }

        public override void Pulse()
        {
            using (StyxWoW.Memory.AcquireFrame())
            {
                Helpers.General.UpdateHelpers();
            }
        }
    }
}
