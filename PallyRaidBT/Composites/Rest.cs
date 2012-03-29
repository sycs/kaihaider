//////////////////////////////////////////////////
//                  Rest.cs                     //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from PallyRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Drawing;
using CommonBehaviors.Actions;
using Styx;
using Styx.Helpers;
using Styx.Logic.Inventory;
using Styx.Logic.Pathing;
using TreeSharp;

namespace PallyRaidBT.Composites
{
    class Rest
    {
        static public Composite CreateRestBehavior()
        {
            return new Decorator(ret => !StyxWoW.Me.IsSwimming && !StyxWoW.Me.IsGhost &&
                                        StyxWoW.Me.IsAlive && !StyxWoW.Me.Mounted &&
                                        Helpers.Area.mLocation != Helpers.Enumeration.LocationContext.Raid &&
                                        Helpers.Area.mLocation != Helpers.Enumeration.LocationContext.HeroicDungeon,
                new PrioritySelector(
                    new Decorator(ret => Helpers.Spells.IsAuraActive(StyxWoW.Me, "Food") && StyxWoW.Me.HealthPercent <= 90,
                        new ActionAlwaysSucceed()),

                    new Decorator(ret => Consumable.GetBestFood(true) != null && StyxWoW.Me.HealthPercent <= 75,
                        new PrioritySelector(
                            new Decorator(ret => StyxWoW.Me.IsMoving,
                                new Action(ret => Navigator.PlayerMover.MoveStop())
                            ),

                            new Action(ret => Styx.Logic.Common.Rest.FeedImmediate())
                        )
                    ),

                    new Decorator(ret => StyxWoW.Me.HealthPercent <= 30,
                        new Action(ret => Logging.Write(Color.Orange, "No food, waiting to heal!")))
                )
            );
        }
    }
}
