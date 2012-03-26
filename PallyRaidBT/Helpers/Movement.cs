//////////////////////////////////////////////////
//               Helpers/Movement.cs            //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using Styx;
using Styx.Logic.Pathing;
using TreeSharp;

namespace PallyRaidBT.Helpers
{
    public delegate WoWPoint WoWPointDelegate(object context);

    static class Movement
    {
        static public Composite MoveToLoc(WoWPointDelegate loc)
        {
            return new PrioritySelector(
                new Decorator(ret => loc(ret).Distance(StyxWoW.Me.Location) > 5,
                    new Action(ret => Navigator.MoveTo(loc(ret)))),

                new Decorator(ret => loc(ret).Distance(StyxWoW.Me.Location) <= 5 && StyxWoW.Me.IsMoving,
                    new Action(ret => Navigator.PlayerMover.MoveStop()))
            );
        }

        static public Composite MoveToUnit(WoWUnitDelegate unit)
        {
            return new PrioritySelector(
                new Decorator(ret => !unit(ret).IsWithinMeleeRange,
                    new Action(ret => Navigator.MoveTo(unit(ret).Location))),

                new Decorator(ret => unit(ret).IsWithinMeleeRange && StyxWoW.Me.IsMoving,
                    new Action(ret => Navigator.PlayerMover.MoveStop()))
            );
        }

        static public Composite FaceUnit(WoWUnitDelegate unit)
        {
            return new Decorator(ret => !StyxWoW.Me.IsSafelyFacing(unit(ret)),
                new Action(ret => unit(ret).Face())
            );
        }

        static public Composite MoveToAndFaceUnit(WoWUnitDelegate unit)
        {
            return new PrioritySelector(
                MoveToUnit(unit),
                FaceUnit(unit)
            );
        }
    }
}
