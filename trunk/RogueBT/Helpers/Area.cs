﻿//////////////////////////////////////////////////
//                 Area.cs                      //
//      Part of RogueBT by kaihaider        //
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

namespace RogueBT.Helpers
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

                    return Helpers.Rogue.me.CurrentTarget.CreatureRank == WoWUnitClassificationType.WorldBoss ||
                           (Helpers.Rogue.me.CurrentTarget.Level == 88 &&
                           Helpers.Rogue.me.CurrentTarget.CreatureRank == WoWUnitClassificationType.Elite);

                case Enum.LocationContext.HeroicDungeon:

                    return Helpers.Rogue.me.CurrentTarget.Level >= 87 && 
                           (Helpers.Rogue.me.CurrentTarget.CreatureRank == WoWUnitClassificationType.Elite ||
                           Helpers.Rogue.me.CurrentTarget.CreatureRank == WoWUnitClassificationType.Rare ||
                           Helpers.Rogue.me.CurrentTarget.CreatureRank == WoWUnitClassificationType.RareElite ||
                           Helpers.Rogue.me.CurrentTarget.CreatureRank == WoWUnitClassificationType.WorldBoss);

                case Enum.LocationContext.Battleground:

                    return Helpers.Rogue.me.CurrentTarget.IsPlayer;

                default:

                    return Helpers.Rogue.me.CurrentTarget.CreatureRank == WoWUnitClassificationType.Elite ||
                           Helpers.Rogue.me.CurrentTarget.CreatureRank == WoWUnitClassificationType.Rare ||
                           Helpers.Rogue.me.CurrentTarget.CreatureRank == WoWUnitClassificationType.RareElite ||
                           Helpers.Rogue.me.CurrentTarget.CreatureRank == WoWUnitClassificationType.WorldBoss;
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

            if(Helpers.Rogue.me.GroupInfo.IsInRaid)
            {
                return Enum.LocationContext.Raid;
            }

            if (Helpers.Rogue.me.IsInInstance && Helpers.Rogue.me.Level == 90)
            {
                return Enum.LocationContext.HeroicDungeon;
            }

            if (Helpers.Rogue.me.IsInInstance)
            {
                return Enum.LocationContext.Dungeon;
            }

            return Enum.LocationContext.World;
        }
    }
}