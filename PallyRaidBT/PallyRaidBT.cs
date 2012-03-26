//////////////////////////////////////////////////
//              PallyRaidBT.cs                   //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
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

        private Composite mCombatBehavior;
        private Composite mPullBehavior;
        private Composite mBuffBehavior;
        private Composite mRestBehavior;

        public override void Initialize()
        {
            Settings.Mode.Init();

            TreeRoot.TicksPerSecond = 30;

            Logging.Write(Color.Orange, "");
            Logging.Write(Color.Orange, "PallyRaidBT v" + mCurVersion + " is now operational.");
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

        public override void ShutDown()
        {
            TreeRoot.TicksPerSecond = 15;
        }

        public override void Pulse()
        {
            using (new FrameLock())
            {
                Helpers.Area.Pulse();
                Helpers.Pally.Pulse();
                Helpers.Focus.Pulse();
                Helpers.Trinkets.Pulse();
            }
        }

        public override Composite CombatBehavior
        {
            get
            {
                if (mCombatBehavior == null)
                {
                    switch (Helpers.Pally.mCurrentSpec)
                    {
                        case Helpers.Pally.SpecList.None:

                            Logging.Write(Color.Orange, "  Low level combat tree built.");
                            mCombatBehavior = Composites.Context.None.BuildCombatBehavior();
                            break;

                        case Helpers.Pally.SpecList.Retribution:

                            Logging.Write(Color.Orange, "  Retribution spec combat tree built.");
                            mCombatBehavior = Composites.Context.Retribution.BuildCombatBehavior();
                            break;

                        case Helpers.Pally.SpecList.Holy:

                            Logging.Write(Color.Orange, "  Holy spec combat tree built.");
                            mCombatBehavior = new Action(ret => Logging.Write(Color.Orange, "Invalid spec/context."));
                            break;

                        case Helpers.Pally.SpecList.Protection:

                            Logging.Write(Color.Orange, "  Protection spec combat tree built.");
                            mCombatBehavior = new Action(ret => Logging.Write(Color.Orange, "Invalid spec/context."));
                            break;
                    }
                }

                return mCombatBehavior;
            }
        }

        public override Composite PullBehavior
        {
            get
            {
                if (mPullBehavior == null)
                {
                    switch (Helpers.Pally.mCurrentSpec)
                    {
                        case Helpers.Pally.SpecList.None:

                            Logging.Write(Color.Orange, "  Low level pull tree built.");
                            mPullBehavior = Composites.Context.None.BuildPullBehavior();

                            break;

                        case Helpers.Pally.SpecList.Retribution:

                            Logging.Write(Color.Orange, "  Retribution spec pull tree built.");
                            mPullBehavior = Composites.Context.Retribution.BuildPullBehavior();
                            break;

                        case Helpers.Pally.SpecList.Holy:

                            mPullBehavior = new Action(ret => Logging.Write(Color.Orange, "Invalid spec/context."));
                            break;

                        case Helpers.Pally.SpecList.Protection:

                            mPullBehavior = new Action(ret => Logging.Write(Color.Orange, "Invalid spec/context."));
                            break;
                    }
                }

                return mPullBehavior;
            }
        }


        public override Composite PreCombatBuffBehavior
        {
            get
            {
                if (mBuffBehavior == null)
                {
                    switch (Helpers.Pally.mCurrentSpec)
                    {
                        case Helpers.Pally.SpecList.None:

                            mBuffBehavior = new Action(ret => RunStatus.Failure);
                            break;

                        case Helpers.Pally.SpecList.Retribution:

                            Logging.Write(Color.Orange, "  Retribution buff tree built.");
                            mBuffBehavior = Composites.Context.Retribution.BuildBuffBehavior();
                            break;

                        case Helpers.Pally.SpecList.Holy:

                            mBuffBehavior = new Action(ret => Logging.Write(Color.Orange, "Invalid spec/context."));
                            break;

                        case Helpers.Pally.SpecList.Protection:

                            mBuffBehavior = new Action(ret => Logging.Write(Color.Orange, "Invalid spec/context."));
                            break;
                    }
                }

                return mBuffBehavior;
            }
        }

        public override Composite RestBehavior
        {
            get
            {
                if (mRestBehavior == null)
                {
                    if (Helpers.Area.mLocation == Helpers.Area.LocationContext.World || Helpers.Area.mLocation == Helpers.Area.LocationContext.Undefined)
                    {
                        Logging.Write(Color.Orange, "  Common rest tree created.");
                        mRestBehavior = Composites.Rest.CreateRestBehavior();
                    }
                    else
                    {
                        mRestBehavior = new Action(ret => RunStatus.Failure);
                    }
                }

                return mRestBehavior;
            }
        }
    }
}
