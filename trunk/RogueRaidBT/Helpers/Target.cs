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
using Styx.Logic;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace RogueRaidBT.Helpers
{
    static class Target
    {
        static public IEnumerable<WoWUnit> mNearbyEnemyUnits { get; private set; }

        static public void Pulse()
        {
            if (StyxWoW.IsInGame != false)
            mNearbyEnemyUnits = ObjectManager.GetObjectsOfType<WoWUnit>(true, false)
                                    .Where(unit =>
                                        unit.IsAlive
                                        && !unit.IsNonCombatPet
                                        && !unit.IsCritter
                                        && (unit.IsTargetingMeOrPet
                                           || unit.IsTargetingMyPartyMember
                                           || unit.IsTargetingMyRaidMember
                                           || unit.IsPlayer)
                                        && unit.Distance <= 40
                                        && !unit.IsFriendly)
                                    .OrderBy(unit => unit.Distance).ToList();
        }

        static public Composite EnsureValidTarget()
        {
            return new Decorator(ret => StyxWoW.Me.CurrentTarget == null || !StyxWoW.Me.CurrentTarget.IsAlive //||
                                        ,
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
                    var botBaseUnit = Targeting.Instance.FirstUnit;

                    if (botBaseUnit != null && botBaseUnit.IsAlive &&
                        !botBaseUnit.IsFriendly)
                    {
                        Logging.Write(Color.Orange, "Changing target to " + botBaseUnit.Name);
                        botBaseUnit.Target();
                    }
                    else
                    {
                        var nextUnit = mNearbyEnemyUnits.FirstOrDefault();

                        if (nextUnit != null)
                        {
                            Logging.Write(Color.Orange, "Changing target to " + nextUnit.Name);
                            nextUnit.Target();
                        }
                    }
                }
            );
        }
    }
}
