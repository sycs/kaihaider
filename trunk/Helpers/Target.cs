//////////////////////////////////////////////////
//                Target.cs                     //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Styx;
using Styx.Helpers;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;
using Styx.Common;

namespace RogueRaidBT.Helpers
{
    static class Target
    {
        static public IEnumerable<WoWUnit> mNearbyEnemyUnits { get; private set; }


        static public WoWUnit botBaseUnit { get; private set; }

        static public void Pulse()
        {
            botBaseUnit = Targeting.Instance.FirstUnit;

            if (StyxWoW.IsInGame) //if no tar then any incombat
            mNearbyEnemyUnits = ObjectManager.GetObjectsOfType<WoWUnit>(true, false)
                                    .Where(unit =>
                                        unit.IsAlive
                                        && !unit.IsNonCombatPet
                                        && !unit.IsCritter
                                        && (unit.IsTargetingMeOrPet 
                                           || unit.IsTargetingMyPartyMember
                                           || unit.IsTargetingMyRaidMember
                                           || unit.IsTargetingAnyMinion
                                           || unit == Styx.CommonBot.POI.BotPoi.Current.AsObject
                                           || unit.IsPlayer
                                           || unit == botBaseUnit
                                           || unit.TaggedByMe)
                                       && unit.Distance <= 40
                                        &&  !(unit.IsFlying || unit.Distance2DSqr < 5 * 5 &&
                                                System.Math.Abs(StyxWoW.Me.Z - unit.Z) >= 5)
                                        && !unit.IsFriendly)
                                    .OrderBy(unit => unit.Distance).ToList();
        }

        static public Composite EnsureValidTarget()
        {
           // if (!StyxWoW.Me.Combat ) Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "Targeting..");

            return new Decorator(ret => !mNearbyEnemyUnits.Contains(Rogue.mTarget) && !BotManager.Current.Name.Equals("BGBuddy") ,//StyxWoW.Me.CurrentTarget == null || !StyxWoW.Me.CurrentTarget.IsAlive
                                        
                GetNewTarget()
            );
        }

        static public Composite EnsureBestPvPTarget()
        {
            return new Action();
        }

        static private Composite GetNewTarget()
        {
            return new Action(ret =>
                {
                    //var botBaseUnit = Targeting.Instance.FirstUnit;

                    /*if (botBaseUnit != null && botBaseUnit.IsAlive && !botBaseUnit.IsFriendly)
                    {
                        Logging.Write(LogLevel.Normal, "Changing target to " + botBaseUnit.Name);
                        botBaseUnit.Target();
                    }
                    else **/

                    var nextUnit = mNearbyEnemyUnits.FirstOrDefault();

                    if (nextUnit != null)
                    {
                        Logging.Write(LogLevel.Normal, "Changing target to " + nextUnit.Name);
                        nextUnit.Target();
                    }
                    else StyxWoW.Me.ClearTarget();
                    
                }
            );
        }
    }
}
