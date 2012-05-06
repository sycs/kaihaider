//////////////////////////////////////////////////
//              RogueRaidBT.cs                  //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System;
using System.Drawing;
using RogueRaidBT.Composites;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using TreeSharp;

namespace RogueRaidBT
{
    class RogueRaidBT : CombatRoutine
    {
        public Version mCurVersion = new Version(0, 2);

        public override string Name { get { return "RogueRaidBT v" + mCurVersion; } }
        public override WoWClass Class { get { return WoWClass.Rogue; } }
        public override bool WantButton { get { return true; } }

        public override Composite CombatBehavior { get { return new PerfDec(Composites.Composites.BuildCombatBehavior()); } }
        public override Composite PullBehavior { get { return Composites.Composites.BuildPullBehavior(); } }
        public override Composite PreCombatBuffBehavior { get { return Composites.Composites.BuildBuffBehavior(); } }
        public override Composite RestBehavior { get { return Composites.Rest.BuildRestBehavior(); } }

        public override void Initialize()
        {
            Logging.Write(Color.Orange, "");
            Logging.Write(Color.Orange, "RogueRaidBT v" + mCurVersion + " is now operational.");
            Logging.Write(Color.Orange, "");
            Logging.Write(Color.Orange, "Your feedback is appreciated. Please leave some in the forum thread at:");
            Logging.Write(Color.Orange, "http://www.thebuddyforum.com/honorbuddy-forum/classes/rogue/32282-release-mutaraid-cc.html");
            Logging.Write(Color.Orange, "");
            Logging.Write(Color.Orange, "Enjoy topping the DPS meters!");
            Logging.Write(Color.Orange, "");
        }

        public override void OnButtonPress()
        {
            var configUi = new UI.Config();
            configUi.Show();
        }

        public override void Pulse()
        {
            Helpers.General.UpdateHelpers();
        }
    }
}
