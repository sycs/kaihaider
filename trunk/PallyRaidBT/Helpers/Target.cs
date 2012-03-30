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
using Styx.Logic;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace PallyRaidBT.Helpers
{
    class Target
    {
        static public IEnumerable<WoWUnit> mNearbyEnemyUnits { get; private set; }
        
        
        static public void Pulse()
        {
            mNearbyEnemyUnits = ObjectManager.GetObjectsOfType<WoWUnit>(true, false)
                                    .Where(unit =>
                                        !unit.IsFriendly
                                        && unit.IsAlive
                                        && (unit.IsTargetingMeOrPet
                                           || unit.IsTargetingMyPartyMember
                                           || unit.IsTargetingMyRaidMember
                                           || unit.IsPlayer)
                                        && !unit.IsNonCombatPet
                                        && !unit.IsCritter
                                        && unit.Distance <= 40)
                                    .OrderBy(unit => unit.Distance).ToList();
        }

        static public Composite EnsureValidTarget()
        {
            return new Decorator(ret => StyxWoW.Me.CurrentTarget == null || !StyxWoW.Me.CurrentTarget.IsAlive,
                GetNewTarget()
            );
        }

        static public Composite EnsureBestPvPTarget()
        {
            return new Action();
        }

        static public WoWUnit FindLowestThreat()
        {
            foreach (WoWPlayer d in StyxWoW.Me.PartyMembers)
            {
                if (d.Guid != StyxWoW.Me.Guid)
                {
                    if (d.CurrentTarget != null && d.CurrentTarget.IsAlive && !d.CurrentTarget.IsPlayer && (d.CurrentTarget.IsTargetingMyPartyMember || d.IsTargetingMyRaidMember || d.IsTargetingMeOrPet))
                    {
                        if (d.CurrentTarget.ThreatInfo.RawPercent > 85 && d.CurrentTarget.ThreatInfo.ThreatValue > 1000)
                        {
                            return d.CurrentTarget;
                        }
                    }
                }
            }
            return null;
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
