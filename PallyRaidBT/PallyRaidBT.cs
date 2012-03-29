//////////////////////////////////////////////////
//              PallyRaidBT.cs                   //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from PallyRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System;
using System.Drawing;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using TreeSharp;
using Action = TreeSharp.Action;

namespace PallyRaidBT
{
    class PallyRaidBT : CombatRoutine
    {
        public Version mCurVersion = new Version(0, 0);

        public override sealed string Name { get { return "PallyRaidBT v" + mCurVersion; } }
        public override WoWClass Class { get { return WoWClass.Paladin; } }
        public override bool WantButton { get { return true; } }

        public override Composite CombatBehavior { get { return Composites.Composites.BuildCombatBehavior(); } }
        public override Composite PullBehavior { get { return Composites.Composites.BuildPullBehavior(); } }
        public override Composite PreCombatBuffBehavior { get { return Composites.Composites.BuildBuffBehavior(); } }
        public override Composite RestBehavior { get { return Composites.Rest.CreateRestBehavior(); } }

        public override void Initialize()
        {
            Logging.Write(Color.Orange, "");
            Logging.Write(Color.Orange, "PallyRaidBT v" + mCurVersion + " is now operational.");
            Logging.Write(Color.Orange, "");
            Logging.Write(Color.Orange, "Your feedback is appreciated. Please leave some in the forum thread at:");
            Logging.Write(Color.Orange, "--create forum--");
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
            using (new FrameLock())
            {
                Helpers.Target.Pulse();
                Helpers.Area.Pulse();
                Helpers.Pally.Pulse();
                Helpers.Focus.Pulse();
                Helpers.Specials.Pulse();

                Helpers.Target.EnsureValidTarget();
            }
        }
    }
}
