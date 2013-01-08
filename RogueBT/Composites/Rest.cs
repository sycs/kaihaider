//////////////////////////////////////////////////
//                  Rest.cs                     //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Drawing;
using Honorbuddy.Resources;
using CommonBehaviors.Actions;
using Styx;
using Styx.Pathing;
using Styx.Helpers;
using Styx.CommonBot.Inventory;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.Common;

namespace RogueBT.Composites
{
    internal static class Rest
    {
        public static Composite BuildRestBehavior()
        {
            return new Decorator(ret => !StyxWoW.Me.IsSwimming && !StyxWoW.Me.IsGhost
                                        && StyxWoW.Me.IsAlive && !StyxWoW.Me.Mounted
                                        && Styx.CommonBot.POI.BotPoi.Current.Type != Styx.CommonBot.POI.PoiType.Loot
                                        && Helpers.Area.mLocation != Helpers.Enum.LocationContext.Raid
                                        && Helpers.Area.mLocation != Helpers.Enum.LocationContext.HeroicDungeon,
                                 new PrioritySelector(
                                             Helpers.Spells.CastSelf("Stealth", ret => Helpers.Spells.IsAuraActive(StyxWoW.Me, "Food") && Helpers.Rogue.mHP <= 90
                                                 && !StyxWoW.Me.HasAura("Stealth") && !SpellManager.GlobalCooldown),
                                     new Decorator(
                                         ret =>
                                         Helpers.Spells.IsAuraActive(StyxWoW.Me, "Food") && Helpers.Rogue.mHP <= 90,
                                         new ActionAlwaysSucceed()),
                                     Helpers.Spells.CastSelf("Recuperate", ret => StyxWoW.Me.RawComboPoints >= 1 &&
                                                                                  !Helpers.Spells.IsAuraActive(
                                                                                      StyxWoW.Me, "Recuperate") &&
                                                                                  Helpers.Rogue.CheckSpamLock()),
                                     new Decorator(
                                         ret => Consumable.GetBestFood(true) != null && Helpers.Rogue.mHP <= 70,
                                         new PrioritySelector(
                                             new Decorator(ret => StyxWoW.Me.IsMoving,
                                                           new Action(ret => Navigator.PlayerMover.MoveStop())
                                                 ),
                                             Helpers.Spells.CastSelf("Stealth", ret => !StyxWoW.Me.HasAura("Stealth")),
                                             new Action(ret =>
                                             {
                                                 Styx.CommonBot.Rest.FeedImmediate();
                                Helpers.Rogue.CreateWaitForLagDuration();
                                Helpers.Rogue.CreateWaitForLagDuration();
                                             })
                                             )
                                         ),

                                     new Decorator(ret => Helpers.Rogue.mHP <= 30,
                                                   new PrioritySelector(
                                                       Helpers.Spells.CastSelf("Stealth",
                                                                               ret => !StyxWoW.Me.HasAura("Stealth")),
                                                       new Action(
                                                           ret =>
                                                           Logging.Write(LogLevel.Normal, "No food, waiting to heal!"))
                                                       )
                                         )
                                     )
                );
        }
    }
}