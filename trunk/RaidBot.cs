//This BotBase was created by Apoc, I take no credit for anything within this code
//I just changed "!StyxWoW.Me.CurrentTarget.IsFriendly" to "!StyxWoW.Me.CurrentTarget.IsHostile"
//For the purpose of allowing RaidBot to work within Arenas
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreeSharp;
using Styx;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Profiles;
using Styx.Logic.Combat;

namespace RaidBot
{
    public class RaidBot : BotBase
    {
        private byte _oldTps;
        private Composite _root;
        public override string Name { get { return "Raid Bot"; } }
        public override Composite Root { get { return _root ?? (_root = new PrioritySelector(CreateRootBehavior())); } }
        public override PulseFlags PulseFlags { get { return PulseFlags.Objects | PulseFlags.Lua; } }
        public override void Start() { _oldTps = TreeRoot.TicksPerSecond; TreeRoot.TicksPerSecond = 30; if (ProfileManager.CurrentProfile == null) ProfileManager.LoadEmpty(); }
        private static Composite CreateRootBehavior()
        {
            return
                new PrioritySelector(
                    new Decorator(
                        ret => !StyxWoW.Me.Combat,
                        new PrioritySelector(
                            new Decorator(
                                ret => RoutineManager.Current.PreCombatBuffBehavior != null,
                                RoutineManager.Current.PreCombatBuffBehavior))),
                                new Decorator(
                                    ret => StyxWoW.Me.Combat && StyxWoW.Me.GotTarget && (!StyxWoW.Me.CurrentTarget.IsHostile || !StyxWoW.Me.CurrentTarget.IsFriendly),
                                    new LockSelector(
                                        RoutineManager.Current.HealBehavior,
                                        RoutineManager.Current.CombatBuffBehavior,
                                        RoutineManager.Current.CombatBehavior)));
        }
        public override void Stop() { TreeRoot.TicksPerSecond = _oldTps; }
        private class LockSelector : PrioritySelector { public LockSelector(params Composite[] children) : base(children) { }
            public override RunStatus Tick(object context) { using (new FrameLock()) { return base.Tick(context); } }
        }
    }
}
