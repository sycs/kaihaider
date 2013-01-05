//////////////////////////////////////////////////
//                 Area.cs                      //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Drawing;
using System.Windows.Media;
using Styx;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.Common;
using Styx.WoWInternals;

namespace RogueRaidBT.Helpers
{
    static class Area
    {
        static public Enum.LocationContext mLocation { get; private set; }

        static public void Pulse()
        {
            Enum.LocationContext curLocation = !Settings.Mode.mOverrideContext ? GetCurrentLocation() : Settings.Mode.mLocationSettings;

            if (mLocation != curLocation)
            {
                mLocation = curLocation;

                Logging.Write(LogLevel.Normal, "");
                Logging.Write(LogLevel.Normal, "Your current context is {0}.", mLocation);
                Logging.Write(LogLevel.Normal, "");
            }
        }

        static public bool IsCurTargetSpecial()
        {
            switch (mLocation)
            {
                case Enum.LocationContext.Raid:

                    return StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.WorldBoss ||
                           (StyxWoW.Me.CurrentTarget.Level == 88 &&
                           StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.Elite);

                case Enum.LocationContext.HeroicDungeon:

                    return StyxWoW.Me.CurrentTarget.Level >= 87 && 
                           (StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.Elite ||
                           StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.Rare ||
                           StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.RareElite ||
                           StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.WorldBoss);

                case Enum.LocationContext.Battleground:

                    return StyxWoW.Me.CurrentTarget.IsPlayer;

                default:

                    return StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.Elite ||
                           StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.Rare ||
                           StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.RareElite ||
                           StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.WorldBoss;
            }
        }

        static private Enum.LocationContext GetCurrentLocation()
        {
            if (Battlegrounds.IsInsideBattleground && Battlegrounds.Current == BattlegroundType.None)
            {
                return Enum.LocationContext.Arena;
            }

            if (Battlegrounds.IsInsideBattleground)
            {
                return Enum.LocationContext.Battleground;
            }

            if(StyxWoW.Me.GroupInfo.IsInRaid)
            {
                return Enum.LocationContext.Raid;
            }

            if (StyxWoW.Me.IsInInstance && StyxWoW.Me.Level == 85)
            {
                return Enum.LocationContext.HeroicDungeon;
            }

            if (StyxWoW.Me.IsInInstance)
            {
                return Enum.LocationContext.Dungeon;
            }

            return Enum.LocationContext.World;
        }
    }
}
