//////////////////////////////////////////////////
//                  Rest.cs                     //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Linq;
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
            return new Decorator(ret => !Helpers.Rogue.me.IsSwimming && !Helpers.Rogue.me.IsGhost
                                        && Helpers.Rogue.me.IsAlive && !Helpers.Rogue.me.Mounted
                                        && Styx.CommonBot.POI.BotPoi.Current.Type != Styx.CommonBot.POI.PoiType.Loot
                                        && Helpers.Area.mLocation != Helpers.Enum.LocationContext.Raid
                                        && Helpers.Area.mLocation != Helpers.Enum.LocationContext.HeroicDungeon,
                                 new PrioritySelector(

                                     new Decorator(ret => Helpers.Aura.Stealth && Helpers.Rogue.mHP < 20 && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 20) < 0,
                                                   new PrioritySelector(
                                                       new Action(
                                                           ret =>{
                                                               Logging.Write(LogLevel.Diagnostic, "Waiting so I don't get ganked by those mobs");
                                                               //new WaitContinue(System.TimeSpan.FromSeconds(2), ret2 => false, new ActionAlwaysSucceed());
                                                           })
                                                       )
                                         ),
                                             Helpers.Spells.CastSelf("Stealth", ret => Helpers.Spells.IsAuraActive(Helpers.Rogue.me, "Food") && Helpers.Rogue.mHP <= 90
                                                 && !Helpers.Rogue.me.HasAura("Stealth") && Helpers.Rogue.me.GetAuraByName("Food").TimeLeft.TotalSeconds < 18 && !SpellManager.GlobalCooldown),
                                     new Decorator(
                                         ret =>
                                         Helpers.Spells.IsAuraActive(Helpers.Rogue.me, "Food") && Helpers.Rogue.mHP <= 90,
                                         new ActionAlwaysSucceed()),
                                     Helpers.Spells.CastSelf("Recuperate", ret => Helpers.Rogue.me.RawComboPoints >= 1 &&
                                                                                  !Helpers.Spells.IsAuraActive(
                                                                                      Helpers.Rogue.me, "Recuperate") &&
                                                                                  Helpers.Rogue.CheckSpamLock()),
                                     new Decorator(
                                         ret => Consumable.GetBestFood(true) != null && Helpers.Rogue.mHP <= 70,
                                         new PrioritySelector(
                                             new Decorator(ret => Helpers.Rogue.me.IsMoving,
                                                           new Action(ret => Navigator.PlayerMover.MoveStop())
                                                 ),
                                             new Action(ret =>
                                             {
                                                 Styx.CommonBot.Rest.FeedImmediate();
                                new WaitContinue(System.TimeSpan.FromSeconds(2), ret2 => false, new ActionAlwaysSucceed());
                                             })
                                             )
                                         ),

                                     new Decorator(ret => Helpers.Rogue.mHP <= 30,
                                                   new PrioritySelector(
                                                       Helpers.Spells.CastSelf("Stealth",
                                                                               ret => !Helpers.Rogue.me.HasAura("Stealth")),
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