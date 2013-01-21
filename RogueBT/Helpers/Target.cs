//////////////////////////////////////////////////
//                   Target.cs                  //
//         Part of RogueBT by kaihaider         //
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
        static public IEnumerable<WoWUnit> mHostileUnits { get; private set; }

       // static public List<ulong> mBlacklist { get; private set; }

        static public WoWUnit botBaseUnit { get; private set; }
        static public WoWUnit BlindCCUnit { get; private set; }
        static public WoWUnit GougeCCUnit { get; private set; }
        static public WoWUnit SapCCUnit { get; private set; }
        static public ulong newTargetGUID { get; private set; }


        static public void Pulse()
        {
            //BlindCCUnit = null;
            //GougeCCUnit = null;
            //SapCCUnit = null;
            botBaseUnit = Targeting.Instance.FirstUnit;
            mHostileUnits = null;


            switch (Helpers.Area.mLocation)
            {
                case Enum.LocationContext.Battleground:
                     mNearbyEnemyUnits = ObjectManager.GetObjectsOfType<WoWUnit>(true, false)
                                    .Where(unit =>
                                        unit.IsAlive
                                        && unit.IsPlayer
                                        && unit.ToPlayer().IsHorde != Helpers.Rogue.me.IsHorde ///from singular
                                       && unit.Distance <= 40)
                                    .OrderBy(unit => unit.Distance).ToList();
                    break;

                case Enum.LocationContext.World:
                    {
                        mHostileUnits = ObjectManager.GetObjectsOfType<WoWUnit>(true, false)
                                                            .Where(unit =>
                                                                unit.IsAlive
                                                                && !unit.IsNonCombatPet
                                                                && !unit.IsCritter
                                                                && !unit.IsPetBattleCritter
                                                                && unit.IsHostile
                                                                && unit.Distance <= 40
                                                                && unit.Attackable
                                                                && unit.CanSelect
                                                                && !unit.IsDead
                                                                && !(System.Math.Abs(Helpers.Rogue.me.Z - unit.Z) >= 3 && Helpers.Movement.IsAboveTheGround(unit)))
                                                            .OrderBy(unit => unit.Distance).ToList();
                        mNearbyEnemyUnits = ObjectManager.GetObjectsOfType<WoWUnit>(true, false)
                                        .Where(unit => 
                                            unit.IsAlive
                                            && !unit.IsNonCombatPet
                                            && !unit.IsCritter
                                            && !unit.IsPetBattleCritter
                                            && (unit.Guid == newTargetGUID
                                               || unit.IsTargetingMeOrPet
                                               || unit.IsTargetingMyPartyMember
                                               || unit.IsTargetingMyRaidMember
                                               || unit.IsTargetingAnyMinion
                                               || unit == Styx.CommonBot.POI.BotPoi.Current.AsObject
                                               || unit.IsPlayer && unit.ToPlayer().IsHorde != Helpers.Rogue.me.IsHorde ///from singular
                                               || unit == botBaseUnit
                                               || unit.TaggedByMe
                                               || unit.HasAura("Blind")
                                               || unit.HasAura("Gouge")
                                               || unit.HasAura("Sap"))
                                            && unit.Distance <= 40
                                            && unit.Attackable
                                            && unit.CanSelect
                                            && !unit.IsDead
                                            && !(System.Math.Abs(Helpers.Rogue.me.Z - unit.Z) >= 3 && Helpers.Movement.IsAboveTheGround(unit))
                                            && !unit.IsFriendly)
                                        .OrderBy(unit => unit.Distance).ToList();
                        break;
                    }
                default:
                    mNearbyEnemyUnits = ObjectManager.GetObjectsOfType<WoWUnit>(true, false)
                                    .Where(unit =>
                                        unit.IsAlive
                                        && !unit.IsNonCombatPet
                                        && !unit.IsCritter
                                        && !unit.IsPetBattleCritter
                                        && (unit.IsTargetingMeOrPet
                                           || unit == Styx.CommonBot.POI.BotPoi.Current.AsObject
                                           || unit.IsTargetingMyPartyMember
                                           || unit.IsTargetingMyRaidMember
                                           || unit.IsTargetingAnyMinion
                                           || unit.IsPlayer && unit.ToPlayer().IsHorde != Helpers.Rogue.me.IsHorde ///from singular
                                           || unit == botBaseUnit
                                           || unit.TaggedByMe)
                                       && unit.Distance <= 40
                                        && unit.Attackable
                                        && unit.CanSelect
                                        && !unit.IsDead
                                        && !(System.Math.Abs(Helpers.Rogue.me.Z - unit.Z) >= 4 && Helpers.Movement.IsAboveTheGround(unit))
                                        && !unit.IsFriendly)
                                    .OrderBy(unit => unit.Distance).ToList();
                    break;

            }
                
        }

        static public Composite EnsureValidTarget()
        {
          //  if (Helpers.Rogue.mTarget != null && Helpers.Rogue.me.Combat && Styx.CommonBot.POI.BotPoi.Current.Type.Equals(Styx.CommonBot.POI.PoiType.Hotspot)) 
          //  Styx.CommonBot.POI.BotPoi.Current = new Styx.CommonBot.POI.BotPoi(Helpers.Rogue.mTarget, Styx.CommonBot.POI.PoiType.Kill);
            return new Decorator(ret => Settings.Mode.mTargeting 
                && (Rogue.mTarget == null || !Rogue.mTarget.IsAlive
                || mNearbyEnemyUnits != null && !mNearbyEnemyUnits.Contains(Rogue.mTarget)
                || Rogue.mTarget.Distance > 25 && Helpers.Rogue.mHP < 60 && mNearbyEnemyUnits != null && mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 0
                || Rogue.mTarget.Distance > 30 && Helpers.Rogue.mHP > 60 && Movement.IsAboveTheGround(Rogue.mTarget) && System.Math.Abs(Helpers.Rogue.me.Z - Helpers.Rogue.mTarget.Z) >= 4 && Helpers.Rogue.mTarget.CurrentTarget != Helpers.Rogue.me
                || Rogue.mTarget.IsFriendly || Rogue.mTarget.HasAura("Sap") && mNearbyEnemyUnits.Count() > 1),
                GetNewTarget()
            );
        }
        static public Composite BlindAdd()
        {       
            return new Decorator(ret => Settings.Mode.mCrowdControl && Helpers.Spells.CanCast("Blind")
                                        && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 20) > 1
                                        && Helpers.Rogue.mHP < 85,
                new Sequence(
                    new Action(ret =>
                    {
                        var AddsOnMe = mNearbyEnemyUnits.Where(unit =>
                                    unit != Helpers.Rogue.mTarget
                                    && unit.IsTargetingMeOrPet
                                    && unit.CurrentHealth >= (Helpers.Rogue.me.MaxHealth * 0.3)
                                    && unit.Distance <= 15 && (unit.Distance > 10 || Helpers.Rogue.mHP < 45)
                                    && !unit.HasAura("Gouge") && !unit.HasAura("Sap"));

                        BlindCCUnit = AddsOnMe.FirstOrDefault();

                        Logging.Write(LogLevel.Diagnostic, "Setting Blind target");
                    }),

                     Helpers.Spells.Cast("Blind", ret2 => BlindCCUnit != null, ret2 => BlindCCUnit)
                    ));
        }
        static public Composite GougeAdd()
        {
            return new Decorator(ret => Settings.Mode.mCrowdControl && Helpers.Spells.CanCast("Gouge")
                                        && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 1
                                        && Helpers.Rogue.mHP < 85,
                new Sequence(
                    new Action(ret =>
                            {
                                var AddsOnMe = mNearbyEnemyUnits.Where(unit =>
                                    unit != Helpers.Rogue.mTarget
                                    && unit.IsTargetingMeOrPet
                                    && unit.CurrentHealth >= (Helpers.Rogue.me.MaxHealth * 0.3)
                                    && unit.Distance < System.Math.Max(3.5f, Helpers.Rogue.me.CombatReach - 0.1333334f + Helpers.Rogue.mTarget.CombatReach)
                                    && Helpers.Rogue.me.IsFacing(unit.Location)
                                    && !unit.HasAura("Blind") && !unit.HasAura("Sap"));

                                GougeCCUnit = AddsOnMe.FirstOrDefault();

                               Logging.Write(LogLevel.Diagnostic, "Setting Gouge target");
                            }),

                     Helpers.Spells.Cast("Gouge", ret2 => GougeCCUnit != null, ret2 => GougeCCUnit)
                           
                    ));
        }

        static public Composite SapAdd()
        {
            return new Decorator(ret => Settings.Mode.mSap.Equals(Helpers.Enum.Saps.Adds) && Helpers.Spells.CanCast("Sap")
                && !Rogue.me.IsActuallyInCombat && Aura.Stealth && Rogue.mTarget.Distance < 25
                && mHostileUnits != null && mNearbyEnemyUnits.Count(unit => unit.HasAura("Sap")) == 0
                && mHostileUnits.Count(unit => unit.Location.Distance(Rogue.mTarget.Location) <= 25 && IsSappable(unit) && unit.InLineOfSight) > 1,
                new Sequence(
                       new Action(ret =>
                            {
                                var SapAdds = mHostileUnits.Where(unit => unit != Rogue.mTarget
                                    && unit.Location.Distance(Rogue.mTarget.Location) <= 25 && IsSappable(unit) && unit.InLineOfSight)
                                    .OrderBy(unit => unit.Location.Distance(Rogue.mTarget.Location)).ToList();
                               SapCCUnit = SapAdds.FirstOrDefault();

                               Logging.Write(LogLevel.Diagnostic, "Setting sap target");
                            }),
                        new PrioritySelector(
                            Helpers.Spells.Cast("Shadowstep", ret => SapCCUnit != null
                                    && SapCCUnit.Distance > System.Math.Max(3.5f, Helpers.Rogue.me.CombatReach - 0.1333334f + SapCCUnit.CombatReach)
                                    && SapCCUnit.InLineOfSpellSight, ret => SapCCUnit),
                            new Decorator(ret => SapCCUnit != null && Helpers.Rogue.me.IsFacing(Helpers.Rogue.mTarget)
                                    && Helpers.Rogue.mTarget.Distance < System.Math.Max(3.5f, Helpers.Rogue.me.CombatReach - 0.1333334f + Helpers.Rogue.mTarget.CombatReach),
                                new Sequence(
                                    new Action(ret =>
                                    {
                                        if (!Helpers.Rogue.me.MovementInfo.IsStrafing)
                                            Styx.Pathing.Navigator.PlayerMover.MoveStop();
                                        return RunStatus.Success;
                                    }),
                                    new Action(ret => 
                                    {
                                        SpellManager.Cast("Pick Pocket", Helpers.Rogue.mTarget);
                                        Logging.Write(LogLevel.Normal, "Sapping Target, swithcing targets");
                                        SpellManager.Cast("Sap", Helpers.Rogue.mTarget);
                                        // Helpers.Spells.Cast("Sap", ret2 => true);
                                        SapCCUnit.Target();
                                        Helpers.Rogue.mTarget = SapCCUnit;
                                        newTargetGUID = SapCCUnit.Guid;
                                        return RunStatus.Success;
                                   } )
                                   ,new Action(ret =>
                                   {
                                       Helpers.Rogue.CreateWaitForLagDuration();
                                       if( !Helpers.Rogue.me.MovementInfo.IsStrafing )
                                       Styx.Pathing.Navigator.PlayerMover.MoveStop();
                                       return RunStatus.Success;
                                   })
                                   //,new Action(ret =>RunStatus.Failure)
                                   )),
                            new Decorator(ret => SapCCUnit != null && Settings.Mode.mUseMovement && !Helpers.Rogue.me.MovementInfo.IsStrafing
                                    && SapCCUnit.Distance > System.Math.Max(3.5f, Helpers.Rogue.me.CombatReach - 0.1333334f + SapCCUnit.CombatReach),
                                new Action(ret =>
                                {
                                    Styx.Pathing.Navigator.MoveTo(SapCCUnit.Location);
                                    return RunStatus.Success;
                                })),
                            new Decorator(ret => SapCCUnit != null && !Helpers.Rogue.me.IsFacing(SapCCUnit) && Settings.Mode.mUseMovement && !Helpers.Rogue.me.MovementInfo.IsStrafing
                                    && SapCCUnit.Distance < System.Math.Max(3.5f, Helpers.Rogue.me.CombatReach - 0.1333334f + SapCCUnit.CombatReach),
                                new Sequence(
                                    new Action(ret =>
                                    {

                                        if (!Helpers.Rogue.me.MovementInfo.IsStrafing)
                                    Styx.Pathing.Navigator.PlayerMover.MoveStop();
                                    return RunStatus.Success;
                                    }),
                                    new Action(ret =>
                                    {
                                    SapCCUnit.Face();
                                    }))),
                            new Decorator(ret => SapCCUnit != null && Helpers.Rogue.me.IsFacing(SapCCUnit)
                                    && SapCCUnit.Distance < System.Math.Max(3.5f, Helpers.Rogue.me.CombatReach - 0.1333334f + SapCCUnit.CombatReach),
                                new Sequence(
                                    new Action(ret =>
                                    {

                                        if (!Helpers.Rogue.me.MovementInfo.IsStrafing)
                                        Styx.Pathing.Navigator.PlayerMover.MoveStop();
                                        return RunStatus.Success;
                                    }),
                                new Action(ret => 
                                    {

                                    SpellManager.Cast("Pick Pocket", SapCCUnit);
                                    Logging.Write(LogLevel.Normal, "Sapping Add");
                                    SpellManager.Cast("Sap", SapCCUnit);
                                    //Helpers.Spells.Cast("Sap", ret2 => true, ret2 => SapCCUnit);
                                    SapCCUnit = null;
                                    //return RunStatus.Failure;
                                    return RunStatus.Success;
                                    })
                                   ,new Action(ret =>
                                   {
                                       if (!Helpers.Rogue.me.MovementInfo.IsStrafing)
                                           Styx.Pathing.Navigator.PlayerMover.MoveStop();
                                           return RunStatus.Failure;
                                       })
                                   ))
                                ),

                        
                        new Action(ret =>
                        {
                            SapCCUnit = null;
                        })

               )
           );
        }

        static public bool IsSappable(WoWUnit unit) //Helpers.Aura.Stealth && Helpers.Rogue.me.IsSafelyFacing(unit) && unit.Distance < System.Math.Max(3.5f, Helpers.Rogue.me.CombatReach - 0.1333334f + Rogue.mTarget.CombatReach)
        {
            return unit != null  && !unit.HasAura("Sap") && !unit.Combat
                   && (unit.IsBeast || unit.IsDemon || unit.IsDragon || unit.IsHumanoid);

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

                    if (nextUnit != null && nextUnit.IsAlive)
                    {
                        Logging.Write(LogLevel.Normal, "Changing target to " + nextUnit.Name);
                        nextUnit.Target();
                    }
                    else Helpers.Rogue.me.ClearTarget();
                }
            );
        }
    }
}
