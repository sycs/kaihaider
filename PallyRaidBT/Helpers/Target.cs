//////////////////////////////////////////////////
//                Helpers/Target.cs             //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Styx;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace PallyRaidBT.Helpers
{
    class Target
    {
        static private IEnumerable<WoWUnit> mNearbyEnemyUnits 
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>(true, false)
                        .Where(unit =>
                            !unit.IsFriendly
                            && unit.IsAlive
                            && (unit.IsTargetingMeOrPet
                               || unit.IsTargetingMyPartyMember
                               || unit.IsTargetingMyRaidMember
                               || unit.IsPlayer
                               || unit.HealthPercent < 100)
                            && !unit.IsNonCombatPet
                            && !unit.IsCritter
                            && unit.Distance <= 15)
                        .OrderBy(unit => unit.Distance).ToList();
            }
        }

        static public Composite EnsureValidTarget()
        {
            return new Decorator(ret => StyxWoW.Me.CurrentTarget == null || StyxWoW.Me.CurrentTarget.Dead,
                GetNewTarget()
            );
        }

        static public Composite GetNewTarget()
        {
            return new Action(ret =>
                {
                    var unit = mNearbyEnemyUnits.FirstOrDefault();

                    if (unit != null && unit.IsAlive)
                    {
                        Logging.Write(Color.Orange, "Changing target to " + unit.Name);
                        unit.Target();
                    }
                }
            );
        }
    }
}
