//////////////////////////////////////////////////
//                  Rest.cs                     //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Drawing;
using CommonBehaviors.Actions;
using Styx;
using Styx.Helpers;
using Styx.Logic.Inventory;
using Styx.Logic.Pathing;
using TreeSharp;

namespace RogueRaidBT.Composites
{
    static class Rest
    {
        static public Composite BuildRestBehavior()
        {
            return new Decorator(ret => !StyxWoW.Me.IsSwimming && !StyxWoW.Me.IsGhost && 
                                        StyxWoW.Me.IsAlive && !StyxWoW.Me.Mounted &&
                                        Helpers.Area.mLocation != Helpers.Enum.LocationContext.Raid &&
                                        Helpers.Area.mLocation != Helpers.Enum.LocationContext.HeroicDungeon,
                new PrioritySelector(
                    new Decorator(ret => Helpers.Spells.IsAuraActive(StyxWoW.Me, "Food") && Helpers.Rogue.mHP <= 90,
                        new ActionAlwaysSucceed()),

                    Helpers.Spells.CastSelf("Recuperate", ret => StyxWoW.Me.RawComboPoints >= 1 &&
                                                                 !Helpers.Spells.IsAuraActive(StyxWoW.Me, "Recuperate") && 
                                                                 Helpers.Rogue.CheckSpamLock()),

                    new Decorator(ret => Consumable.GetBestFood(true) != null && Helpers.Rogue.mHP <= 5,
                        new PrioritySelector(

                            new Decorator(ret => StyxWoW.Me.IsMoving,
                                new Action(ret => Navigator.PlayerMover.MoveStop())
                            ),

                            Helpers.Spells.CastSelf("Stealth", ret => !StyxWoW.Me.HasAura("Stealth")),

                            new Action(ret => Styx.Logic.Common.Rest.FeedImmediate())
                        )
                    ),

                    new Decorator(ret => Helpers.Rogue.mHP <= 30,
                        new PrioritySelector(
                            Helpers.Spells.CastSelf("Stealth", ret => !StyxWoW.Me.HasAura("Stealth")),
                            new Action(ret => Logging.Write(Color.Orange, "No food, waiting to heal!"))
                        )
                    )
                )
            );
        }
    }
}
