//////////////////////////////////////////////////
//                 Area.cs                      //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Drawing;
using Styx;
using Styx.Helpers;
using Styx.Logic;

namespace PallyRaidBT.Helpers
{
    class Area
    {
        public enum LocationContext
        {
            Undefined = 0,
            Raid,
            HeroicDungeon,
            Dungeon,
            Battleground,
            Arena,
            World
        }

        static public LocationContext mLocation { get; private set; }

        static public void Pulse()
        {
            if (mLocation == GetCurrentLocation()) return;

            if (Settings.Mode.mCurMode != Settings.Mode.Modes.Auto)
            {
                switch (Settings.Mode.mCurMode)
                {
                    case Settings.Mode.Modes.Raid:

                        mLocation = LocationContext.Raid;
                        break;

                    default:

                        mLocation = LocationContext.World;
                        break;
                }
            }
            else
            {
                mLocation = GetCurrentLocation();
            }

            Logging.Write(Color.Orange, "");
            Logging.Write(Color.Orange, "Your current context is {0}.", mLocation);
            Logging.Write(Color.Orange, "");
        }

        static private LocationContext GetCurrentLocation()
        {
	    if ((Battlegrounds.IsInsideBattleground || BotManager.Current.Name == "BGBuddy") && !(Battlegrounds.GetCurrentBattleground()==BattlegroundType.None))
            {
                return LocationContext.Battleground;
            }

            if (Battlegrounds.IsInsideBattleground || BotManager.Current.Name == "BGBuddy" && (Battlegrounds.GetCurrentBattleground()==BattlegroundType.None))
            {
                return LocationContext.Arena;
            }

            if (StyxWoW.Me.IsInParty && StyxWoW.Me.IsInInstance &&
                (BotManager.Current.Name == "Raid Bot" || BotManager.Current.Name == "LazyRaider"))
            {
                return LocationContext.HeroicDungeon;
            }

            if (StyxWoW.Me.IsInParty && StyxWoW.Me.IsInInstance &&
                BotManager.Current.Name != "Raid Bot" && BotManager.Current.Name != "LazyRaider")
            {
                return LocationContext.Dungeon;
            }

            if (StyxWoW.Me.IsInRaid || BotManager.Current.Name == "Raid Bot" || BotManager.Current.Name == "LazyRaider")
            {
                return LocationContext.Raid;
            }

            if (BotManager.Current.Name == "Questing" || BotManager.Current.Name == "Grind Bot")
            {
                return LocationContext.World;
            }

            return LocationContext.Undefined;
        }
    }
}
