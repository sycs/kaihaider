//////////////////////////////////////////////////
//                Target.cs                     //
//      Part of RogueBT by kaihaider        //
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

namespace RogueBT.Helpers
{
    static class Target
    {
        static public IEnumerable<WoWUnit> mNearbyEnemyUnits { get; private set; }


        static public WoWUnit botBaseUnit { get; private set; }
        static public WoWUnit BlindCCUnit { get; private set; }
        static public WoWUnit GougeCCUnit { get; private set; }

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
                                        &&  !(unit.IsFlying || unit.Distance2DSqr < 3 * 3 &&
                                                System.Math.Abs(StyxWoW.Me.Z - unit.Z) >= 3)
                                        && !unit.IsFriendly)
                                    .OrderBy(unit => unit.Distance).ToList();
        }

        static public bool GetCCTarget()
        {
            GougeCCUnit = null;
            BlindCCUnit = null;

            if(Helpers.Spells.CanCast("Blind"))
                {
                var AddsOnMe = mNearbyEnemyUnits.Where(unit => 
                                    unit!= Helpers.Rogue.mTarget
                                    && unit.IsTargetingMeOrPet
                                    && unit.Distance <= 15
                                    && !unit.HasAura("Gouge"));

                BlindCCUnit = AddsOnMe.FirstOrDefault();
                }

            if(Helpers.Spells.CanCast("Gouge"))
                {
                var AddsOnMe = mNearbyEnemyUnits.Where(unit => 
                                    unit!= Helpers.Rogue.mTarget
                                    && unit.IsTargetingMeOrPet
                                    && unit.IsWithinMeleeRange
                                    && StyxWoW.Me.IsFacing(unit.Location)
                                    && !unit.HasAura("Blind"));

                GougeCCUnit = AddsOnMe.FirstOrDefault();
                }
            if (GougeCCUnit == null && BlindCCUnit == null)
                return false;
            return true;
        }

        static public Composite EnsureValidTarget()
        {
            return new Decorator(ret => (!mNearbyEnemyUnits.Contains(Rogue.mTarget) || Rogue.mTarget == null) && !BotManager.Current.Name.Equals("BGBuddy"),
                GetNewTarget()
            );
        }

        static public bool IsSappable()
        {
            // if (!StyxWoW.Me.Combat ) Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "Targeting..");
            return Helpers.Aura.Stealth && StyxWoW.Me.IsSafelyFacing(Helpers.Rogue.mTarget)
                   && Helpers.Rogue.mTarget.Distance < 10 && !Helpers.Aura.IsTargetSapped && !Helpers.Rogue.mTarget.Combat
                   && (Helpers.Rogue.mTarget.IsBeast || Helpers.Rogue.mTarget.IsDemon || Helpers.Rogue.mTarget.IsDragon || Helpers.Rogue.mTarget.IsHumanoid);

        }

        static public Composite EnsureBestPvPTarget()
        {
            return new Action();
        }

        static private Composite GetNewTarget()
        {
            return new Action(ret =>
                {
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
