//////////////////////////////////////////////////
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

        static public void Pulse()     //if (Movement.IsPVPSuiteEnabled) Settings.Mode.mUseMovement = false;
        {

            Helpers.Rogue.me = StyxWoW.Me;

            Enum.LocationContext curLocation = !Settings.Mode.mOverrideContext ? GetCurrentLocation() : Settings.Mode.mLocationSettings;

            if (mLocation != curLocation)
            {
                if (curLocation.Equals(Enum.LocationContext.Dungeon))
                {
                    Settings.Mode.mTargeting = false;
                    Settings.Mode.mMoveBackwards = false;
                    Settings.Mode.mCrowdControl = false;
                    Logging.Write(LogLevel.Normal, "Instance Detected: Disabling Crowd Control");
                    Logging.Write(LogLevel.Normal, "Instance Detected: Disabling Targeting");
                    Logging.Write(LogLevel.Normal, "Instance Detected: Disabling Move Backwards");
                }

                if (curLocation.Equals(Enum.LocationContext.Battleground))
                {
                    Settings.Mode.mTargeting = false;
                    Settings.Mode.mMoveBackwards = false;
                    Logging.Write(LogLevel.Normal, "World Detected: Disabling Targeting");
                    Logging.Write(LogLevel.Normal, "Battleground Detected: Disabling Move Backwards");
                }

                if (curLocation.Equals(Enum.LocationContext.World))
                {
                    Settings.Mode.mTargeting = true;
                    Settings.Mode.mCrowdControl = true;
                    Settings.Mode.mMoveBackwards = true;
                    Logging.Write(LogLevel.Normal, "World Detected: Enabling Targeting");
                    Logging.Write(LogLevel.Normal, "World Detected: Enabling Crowd Control");
                    Logging.Write(LogLevel.Normal, "World Detected: Enabling Move Backwards");
                }

                if (mLocation.Equals(Enum.LocationContext.Battleground) && curLocation.Equals(Enum.LocationContext.World)
                    && Helpers.Rogue.me != null && !Helpers.Rogue.me.Mounted)
                    if (Helpers.Rogue.me.Combat && Styx.CommonBot.SpellManager.HasSpell("Vanish") && Helpers.Spells.CanCast("Vanish")) Styx.CommonBot.SpellManager.Cast("Vanish", Helpers.Rogue.me);
                    else if (!Helpers.Rogue.me.Combat && !Helpers.Aura.Stealth)
                    {
                        Styx.Pathing.Navigator.PlayerMover.MoveStop();
                        Styx.CommonBot.SpellManager.Cast("Stealth", Helpers.Rogue.mTarget);
                    }
                    else Styx.Pathing.Navigator.PlayerMover.MoveStop();

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
            //if (Battlegrounds.IsInsideBattleground && Battlegrounds.Current == BattlegroundType.None)
            //{
            //    return Enum.LocationContext.Arena;
            //}

            if (Battlegrounds.IsInsideBattleground)
            {
                return Enum.LocationContext.Battleground;
            }

            if(Helpers.Rogue.me.GroupInfo.IsInRaid)
            {
                return Enum.LocationContext.Raid;
            }

            //if (Helpers.Rogue.me.IsInInstance && Helpers.Rogue.me.Level == 90)
            //{
            //    return Enum.LocationContext.HeroicDungeon;
            //}

            if (Helpers.Rogue.me.IsInInstance)
            {
                return Enum.LocationContext.Dungeon;
            }

            return Enum.LocationContext.World;
        }
    }
}
