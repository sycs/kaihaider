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
        static public WoWUnit newTarget { get; private set; }

        static public bool aoeSafe { get; private set; }

        static public ulong BlindCCUnitGUID { get; private set; }
        static public ulong GougeCCUnitGUID { get; private set; }
        static public ulong SapCCUnitGUID { get; private set; }
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
                        if (!Helpers.Rogue.alwaysStealthCheck && Styx.CommonBot.POI.BotPoi.Current.Type.Equals(Styx.CommonBot.POI.PoiType.Loot)) Helpers.Rogue.alwaysStealthCheck = true;

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
                                            && (unit.IsTargetingMeOrPet
                                               || unit.IsTargetingMyPartyMember
                                               || unit.IsTargetingMyRaidMember
                                               || unit.IsTargetingAnyMinion
                                               || unit == Styx.CommonBot.POI.BotPoi.Current.AsObject
                                               || unit.IsPlayer && unit.ToPlayer().IsHorde != Helpers.Rogue.me.IsHorde ///from singular
                                               || unit == botBaseUnit
                                               || unit.TaggedByMe
                                               || unit.Guid == newTargetGUID
                                               || unit.Guid == BlindCCUnitGUID
                                               || unit.Guid == GougeCCUnitGUID
                                               || unit.Guid == SapCCUnitGUID
                                               //|| unit == newTarget
                                               //|| unit.HasAura("Blind")
                                               //|| unit.HasAura("Gouge")
                                               //|| unit == SapCCUnit
                                               )
                                            && unit.Distance <= 40
                                            && unit.Attackable
                                            && unit.CanSelect
                                            && !unit.IsDead
                                            && !(System.Math.Abs(Helpers.Rogue.me.Z - unit.Z) >= 3 && Helpers.Movement.IsAboveTheGround(unit))
                                            && !unit.IsFriendly)
                                        .OrderBy(unit => unit.Distance).ToList();
                        
                        aoeSafe = mNearbyEnemyUnits.Count(unit =>
                            (unit.Guid == Helpers.Target.BlindCCUnitGUID && unit.HasAura("Blind")
                                || unit.Guid == Helpers.Target.GougeCCUnitGUID && unit.HasAura("Gouge")
                                || unit.Guid == Helpers.Target.SapCCUnitGUID && unit.HasAura("Sap"))
                            && unit.Distance < 11) == 0;

                        break;
                    }
                default:
                    {

                        if (!Helpers.Rogue.IsAoeUsable()) break;
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
                
        }

        //cancel bladefury...
        static public Composite BlindAdd()
        {
            return new Decorator(ret => Settings.Mode.mCrowdControl && Helpers.Spells.CanCast("Blind") && Helpers.Rogue.me != null
                                        && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 20) > 1
                                        && Helpers.Rogue.mHP < 85,
                new Sequence(
                    new Action(ret =>
                    {
                        var AddsOnMe = mNearbyEnemyUnits.Where(unit =>
                                    unit != Helpers.Rogue.mTarget
                                    && unit.IsTargetingMeOrPet
                                    && unit.CurrentHealth >= (Helpers.Rogue.me.MaxHealth * 0.3)
                                    && unit.Distance <= 15 && (unit.Distance > 10 || Helpers.Rogue.mHP < 35 && Helpers.Target.mNearbyEnemyUnits.Count(units => units.Distance <= 20) > 3)
                                    //&& !unit.HasAura("Gouge") && !unit.HasAura("Sap")
                                    && !(unit.Guid == GougeCCUnitGUID && unit.HasAura("Gouge")) && !(unit.Guid == SapCCUnitGUID && unit.HasAura("Sap"))
                                    );

                        BlindCCUnit = AddsOnMe.FirstOrDefault();
                        if (BlindCCUnit != null)
                        BlindCCUnitGUID = BlindCCUnit.Guid;
                        Logging.Write(LogLevel.Diagnostic, "Setting Blind target");
                    }),

                     Helpers.Spells.Cast("Blind", ret2 => BlindCCUnit != null, ret2 => BlindCCUnit)
                    ));
        }
        static public Composite GougeAdd()
        {
            return new Decorator(ret => Settings.Mode.mCrowdControl && Helpers.Spells.CanCast("Gouge") && Helpers.Rogue.me != null
                                        && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 1
                                        && Helpers.Rogue.mHP < 85,
                new Sequence(
                    new Action(ret =>
                            {
                                var AddsOnMe = mNearbyEnemyUnits.Where(unit =>
                                    unit != Helpers.Rogue.mTarget
                                    && unit.IsTargetingMeOrPet
                                    && unit.CurrentHealth >= (Helpers.Rogue.me.MaxHealth * 0.3)
                                    && Helpers.Rogue.me.IsFacing(unit.Location)
                                    //&& !unit.HasAura("Blind") && !unit.HasAura("Sap")
                                    && !(unit.Guid == BlindCCUnitGUID && unit.HasAura("Blind"))
                                    && !(unit.Guid == SapCCUnitGUID && unit.HasAura("Sap"))
                                    && unit.Distance < System.Math.Max(3.5f, Helpers.Rogue.me.CombatReach - 0.1333334f + unit.CombatReach));

                                GougeCCUnit = AddsOnMe.FirstOrDefault();
                                if (GougeCCUnit != null)
                                GougeCCUnitGUID = GougeCCUnit.Guid;
                               Logging.Write(LogLevel.Diagnostic, "Setting Gouge target");
                            }),

                     Helpers.Spells.Cast("Gouge", ret2 => GougeCCUnit != null, ret2 => GougeCCUnit)
                           
                    ));
        }

        static public Composite SapPvPAdd()
        {
            return new Decorator(ret => Settings.Mode.mSap.Equals(Helpers.Enum.Saps.Adds) && Helpers.Spells.CanCast("Sap")
                && !Rogue.me.IsActuallyInCombat && Aura.Stealth && Rogue.mTarget != null && Rogue.mTarget.Distance < 25
                && mHostileUnits != null && mNearbyEnemyUnits.Count(unit => unit.Guid == SapCCUnitGUID && unit.HasAura("Sap")) == 0
                && mHostileUnits.Count(unit => unit.Location.Distance(Rogue.mTarget.Location) <= 25 && IsSappable(unit)) > 1,
                new Sequence(
                       new Action(ret =>
                       {
                           var SapAdds = mHostileUnits.Where(unit => unit != Rogue.mTarget
                               && unit.CurrentHealth >= (Helpers.Rogue.me.MaxHealth * 0.4)
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
                                    new DecoratorContinue(ret => !Helpers.Rogue.me.MovementInfo.IsStrafing,
                                        new Action(ret => Styx.Pathing.Navigator.PlayerMover.MoveStop())),
                                new Action(ret =>
                                {
                                    SpellManager.Cast("Pick Pocket", Helpers.Rogue.mTarget);
                                    return RunStatus.Success;
                                }),
                                new Action(ret =>
                                {
                                    Logging.Write(LogLevel.Normal, "Sapping Target, switching targets");
                                    SpellManager.Cast("Sap", Helpers.Rogue.mTarget);
                                    // Helpers.Spells.Cast("Sap", ret2 => true);
                                }),
                                new Action(ret =>
                                {
                                    newTargetGUID = Helpers.Rogue.mTarget.Guid;
                                    SapCCUnitGUID = Helpers.Rogue.mTarget.Guid;
                                    newTarget = Helpers.Rogue.mTarget;
                                    SapCCUnit.Target();
                                }),
                                new DecoratorContinue(ret => !Helpers.Rogue.me.Combat,
                                new WaitContinue(System.TimeSpan.FromMilliseconds(500), ret2 => false, new CommonBehaviors.Actions.ActionAlwaysSucceed())),
                                new DecoratorContinue(ret => !Helpers.Rogue.me.Combat,
                                new WaitContinue(System.TimeSpan.FromMilliseconds(500), ret2 => false, new CommonBehaviors.Actions.ActionAlwaysSucceed())),
                                new DecoratorContinue(ret => !Helpers.Rogue.me.Combat,
                                new WaitContinue(System.TimeSpan.FromMilliseconds(500), ret2 => false, new CommonBehaviors.Actions.ActionAlwaysSucceed())),
                                new DecoratorContinue(ret => !Helpers.Rogue.me.Combat,
                                new WaitContinue(System.TimeSpan.FromMilliseconds(500), ret2 => false, new CommonBehaviors.Actions.ActionAlwaysSucceed())),
                                new DecoratorContinue(ret => !Helpers.Rogue.me.Combat,
                                new WaitContinue(System.TimeSpan.FromMilliseconds(500), ret2 => false, new CommonBehaviors.Actions.ActionAlwaysSucceed())),
                                new DecoratorContinue(ret => !Helpers.Rogue.me.Combat,
                                new WaitContinue(System.TimeSpan.FromMilliseconds(500), ret2 => false, new CommonBehaviors.Actions.ActionAlwaysSucceed())),
                                new DecoratorContinue(ret => !Helpers.Rogue.me.Combat,
                                new WaitContinue(System.TimeSpan.FromMilliseconds(500), ret2 => false, new CommonBehaviors.Actions.ActionAlwaysSucceed())),
                                new DecoratorContinue(ret => Helpers.Rogue.me.Combat && !SapCCUnit.IsTargetingMeOrPet,
                                    new Action(ret => Helpers.Target.GetNewTarget())),
                                new DecoratorContinue(ret => !Helpers.Rogue.me.Combat,
                                    new Action(ret =>
                                    {
                                        SapCCUnit.Target();
                                        Helpers.Rogue.mTarget = SapCCUnit;
                                        SapCCUnit = newTarget;
                                        newTarget = Helpers.Rogue.mTarget;
                                    }))
                                   ,
                                    new DecoratorContinue(ret => !Helpers.Rogue.me.MovementInfo.IsStrafing,
                                        new Action(ret => Styx.Pathing.Navigator.PlayerMover.MoveStop()))
                //,new Action(ret =>RunStatus.Failure)
                                   )),
                            new Decorator(ret => SapCCUnit != null && Settings.Mode.mUseMovement && !Helpers.Rogue.me.MovementInfo.IsStrafing
                                    && SapCCUnit.Distance > System.Math.Max(3.5f, Helpers.Rogue.me.CombatReach - 0.1333334f + SapCCUnit.CombatReach)
                                    && Helpers.Rogue.SapLock(),
                                new Action(ret =>
                                {
                                    Styx.Pathing.Navigator.MoveTo(SapCCUnit.Location);
                                    return RunStatus.Success;
                                })),
                            new Decorator(ret => SapCCUnit != null && !Helpers.Rogue.me.IsFacing(SapCCUnit) && Settings.Mode.mUseMovement && !Helpers.Rogue.me.MovementInfo.IsStrafing
                                    && SapCCUnit.Distance < System.Math.Max(3.5f, Helpers.Rogue.me.CombatReach - 0.1333334f + SapCCUnit.CombatReach),
                                new Sequence(
                                    new Action(ret => Styx.Pathing.Navigator.PlayerMover.MoveStop()),
                                    new Action(ret => SapCCUnit.Face()))),
                            new Decorator(ret => SapCCUnit != null && Helpers.Rogue.me.IsFacing(SapCCUnit)
                                    && SapCCUnit.Distance < System.Math.Max(3.5f, Helpers.Rogue.me.CombatReach - 0.1333334f + SapCCUnit.CombatReach),
                                new Sequence(
                                    new DecoratorContinue(ret => !Helpers.Rogue.me.MovementInfo.IsStrafing,
                                        new Action(ret => Styx.Pathing.Navigator.PlayerMover.MoveStop())),
                                    new Action(ret =>
                                    {
                                        SpellManager.Cast("Pick Pocket", SapCCUnit);
                                        return RunStatus.Success;
                                    }),
                                    new Action(ret =>
                                    {
                                        Logging.Write(LogLevel.Normal, "Sapping Add");
                                        SpellManager.Cast("Sap", SapCCUnit);
                                        SapCCUnitGUID = SapCCUnit.Guid;
                                        //Helpers.Spells.Cast("Sap", ret2 => true, ret2 => SapCCUnit);
                                        //return RunStatus.Failure;
                                    }),
                                    new DecoratorContinue(ret => Helpers.Rogue.me.Combat,
                                        new Action(ret => Helpers.Target.GetNewTarget())),
                                    new DecoratorContinue(ret => !Helpers.Rogue.me.MovementInfo.IsStrafing,
                                        new Action(ret => Styx.Pathing.Navigator.PlayerMover.MoveStop()))
                                   ))
                                ),
                        new Action(ret =>
                        {
                            //SapCCUnit = null;
                            return RunStatus.Failure;
                        })
               )
           );
        }

        static public Composite SapAdd()
        {
            return new Decorator(ret => Settings.Mode.mSap.Equals(Helpers.Enum.Saps.Adds) && Helpers.Spells.CanCast("Sap")
                && !Rogue.me.IsActuallyInCombat && Aura.Stealth && Rogue.mTarget!= null && Rogue.mTarget.Distance < 25
                && mHostileUnits != null && mNearbyEnemyUnits.Count(unit => unit.Guid == SapCCUnitGUID && unit.HasAura("Sap")) == 0
                && mHostileUnits.Count(unit => unit.Location.Distance(Rogue.mTarget.Location) <= 25 && IsSappable(unit)) > 1,
                new Sequence(
                       new Action(ret =>
                            {
                                var SapAdds = mHostileUnits.Where(unit => unit != Rogue.mTarget
                                    && unit.CurrentHealth >= (Helpers.Rogue.me.MaxHealth * 0.4)
                                    && unit.Location.Distance(Rogue.mTarget.Location) <= 25 && IsSappable(unit) && unit.InLineOfSight)
                                    .OrderBy(unit => unit.Location.Distance(Rogue.mTarget.Location)).ToList();

                               SapCCUnit = SapAdds.FirstOrDefault();

                               Logging.Write(LogLevel.Diagnostic, "Setting sap target");
                            }),
                        new PrioritySelector(
                            new Decorator(ret => SapCCUnit != null && Helpers.Rogue.me.IsFacing(Helpers.Rogue.mTarget)
                                    && Helpers.Rogue.mTarget.Distance < System.Math.Max(3.5f, Helpers.Rogue.me.CombatReach - 0.1333334f + Helpers.Rogue.mTarget.CombatReach),
                                new Sequence(
                                    new DecoratorContinue(ret => !Helpers.Rogue.me.MovementInfo.IsStrafing,
                                        new Action(ret => Styx.Pathing.Navigator.PlayerMover.MoveStop())),
                                new Action(ret => 
                                    {
                                        SpellManager.Cast("Pick Pocket", Helpers.Rogue.mTarget);
                                        return RunStatus.Success;
                                    }),
                                new Action(ret => 
                                    {
                                        Logging.Write(LogLevel.Normal, "Sapping Target, switching targets");
                                        SpellManager.Cast("Sap", Helpers.Rogue.mTarget);
                                        // Helpers.Spells.Cast("Sap", ret2 => true);
                                   } ),
                                new Action(ret =>
                                {
                                    newTargetGUID = Helpers.Rogue.mTarget.Guid;
                                    SapCCUnitGUID = Helpers.Rogue.mTarget.Guid;
                                    newTarget = Helpers.Rogue.mTarget;
                                    SapCCUnit.Target(); 
                                   } ),
                                new DecoratorContinue(ret => !Helpers.Rogue.me.Combat,
                                new WaitContinue(System.TimeSpan.FromMilliseconds(500), ret2 => false, new CommonBehaviors.Actions.ActionAlwaysSucceed())),
                                new DecoratorContinue(ret => !Helpers.Rogue.me.Combat,
                                new WaitContinue(System.TimeSpan.FromMilliseconds(500), ret2 => false, new CommonBehaviors.Actions.ActionAlwaysSucceed())),
                                new DecoratorContinue(ret => !Helpers.Rogue.me.Combat,
                                new WaitContinue(System.TimeSpan.FromMilliseconds(500), ret2 => false, new CommonBehaviors.Actions.ActionAlwaysSucceed())),
                                new DecoratorContinue(ret => !Helpers.Rogue.me.Combat,
                                new WaitContinue(System.TimeSpan.FromMilliseconds(500), ret2 => false, new CommonBehaviors.Actions.ActionAlwaysSucceed())),
                                new DecoratorContinue(ret => !Helpers.Rogue.me.Combat,
                                new WaitContinue(System.TimeSpan.FromMilliseconds(500), ret2 => false, new CommonBehaviors.Actions.ActionAlwaysSucceed())),
                                new DecoratorContinue(ret => !Helpers.Rogue.me.Combat,
                                new WaitContinue(System.TimeSpan.FromMilliseconds(500), ret2 => false, new CommonBehaviors.Actions.ActionAlwaysSucceed())),
                                new DecoratorContinue(ret => !Helpers.Rogue.me.Combat,
                                new WaitContinue(System.TimeSpan.FromMilliseconds(500), ret2 => false, new CommonBehaviors.Actions.ActionAlwaysSucceed())),
                                new DecoratorContinue(ret => Helpers.Rogue.me.Combat && !SapCCUnit.IsTargetingMeOrPet,
                                    new Action(ret => Helpers.Target.GetNewTarget())),
                                new DecoratorContinue(ret => !Helpers.Rogue.me.Combat,
                                    new Action(ret =>
                                    {
                                        SapCCUnit.Target();
                                        Helpers.Rogue.mTarget = SapCCUnit;
                                        SapCCUnit = newTarget;
                                        newTarget = Helpers.Rogue.mTarget;
                                   } ))
                                   ,
                                    new DecoratorContinue(ret => !Helpers.Rogue.me.MovementInfo.IsStrafing,
                                        new Action(ret => Styx.Pathing.Navigator.PlayerMover.MoveStop()))
                                   //,new Action(ret =>RunStatus.Failure)
                                   )),
                            new Decorator(ret => SapCCUnit != null && Settings.Mode.mUseMovement && !Helpers.Rogue.me.MovementInfo.IsStrafing
                                    && SapCCUnit.Distance > System.Math.Max(3.5f, Helpers.Rogue.me.CombatReach - 0.1333334f + SapCCUnit.CombatReach)
                                    && Helpers.Rogue.SapLock(),
                                new Action(ret =>
                                {
                                    Styx.Pathing.Navigator.MoveTo(SapCCUnit.Location);
                                    return RunStatus.Success;
                                })),
                            new Decorator(ret => SapCCUnit != null && !Helpers.Rogue.me.IsFacing(SapCCUnit) && Settings.Mode.mUseMovement && !Helpers.Rogue.me.MovementInfo.IsStrafing
                                    && SapCCUnit.Distance < System.Math.Max(3.5f, Helpers.Rogue.me.CombatReach - 0.1333334f + SapCCUnit.CombatReach),
                                new Sequence(
                                    new Action(ret =>Styx.Pathing.Navigator.PlayerMover.MoveStop()),
                                    new Action(ret => SapCCUnit.Face()))),
                            new Decorator(ret => SapCCUnit != null && Helpers.Rogue.me.IsFacing(SapCCUnit)
                                    && SapCCUnit.Distance < System.Math.Max(3.5f, Helpers.Rogue.me.CombatReach - 0.1333334f + SapCCUnit.CombatReach),
                                new Sequence(
                                    new DecoratorContinue(ret => !Helpers.Rogue.me.MovementInfo.IsStrafing,
                                        new Action(ret => Styx.Pathing.Navigator.PlayerMover.MoveStop())),
                                    new Action(ret => 
                                    {
                                        SpellManager.Cast("Pick Pocket", SapCCUnit);
                                        return RunStatus.Success;
                                    }),
                                    new Action(ret => 
                                    {
                                    Logging.Write(LogLevel.Normal, "Sapping Add");
                                    SpellManager.Cast("Sap", SapCCUnit);
                                    SapCCUnitGUID = SapCCUnit.Guid;
                                    //Helpers.Spells.Cast("Sap", ret2 => true, ret2 => SapCCUnit);
                                    //return RunStatus.Failure;
                                    }),
                                    new DecoratorContinue(ret => Helpers.Rogue.me.Combat,
                                        new Action(ret => Helpers.Target.GetNewTarget())),
                                    new DecoratorContinue(ret => !Helpers.Rogue.me.MovementInfo.IsStrafing,
                                        new Action(ret => Styx.Pathing.Navigator.PlayerMover.MoveStop()))
                                   ))
                                ),
                        new Action(ret =>
                        {
                            //SapCCUnit = null;
                            return RunStatus.Failure;
                        })
               )
           );
        }

        static public bool IsSappable(WoWUnit unit) //Helpers.Aura.Stealth && Helpers.Rogue.me.IsSafelyFacing(unit) && unit.Distance < System.Math.Max(3.5f, Helpers.Rogue.me.CombatReach - 0.1333334f + Rogue.mTarget.CombatReach)
        {
            return unit != null && !(unit.Guid == SapCCUnitGUID && unit.HasAura("Sap")) && !unit.Combat
                   && (unit.IsBeast || unit.IsDemon || unit.IsDragon || unit.IsHumanoid);

        }

        static public Composite EnsureBestPvPTarget()
        {
            return new Action();
        }

        static public Composite EnsureValidTarget()
        {
            //  if (Helpers.Rogue.mTarget != null && Helpers.Rogue.me.Combat && Styx.CommonBot.POI.BotPoi.Current.Type.Equals(Styx.CommonBot.POI.PoiType.Hotspot)) 
            //  Styx.CommonBot.POI.BotPoi.Current = new Styx.CommonBot.POI.BotPoi(Helpers.Rogue.mTarget, Styx.CommonBot.POI.PoiType.Kill);
            return new Decorator(ret => Settings.Mode.mTargeting 
                && (Rogue.mTarget == null || !Rogue.mTarget.IsAlive //|| Rogue.mTarget.IsDead
                //|| mNearbyEnemyUnits != null && !mNearbyEnemyUnits.Contains(Rogue.mTarget)
                || Helpers.Rogue.mHP < 60 && mNearbyEnemyUnits != null && mNearbyEnemyUnits.Count(unit => unit.Distance <= 10) > 0 && Rogue.mTarget.Distance > 25
                || Helpers.Rogue.mHP > 60 && Rogue.mTarget.Distance > 30 && System.Math.Abs(Helpers.Rogue.me.Z - Helpers.Rogue.mTarget.Z) >= 4 && Helpers.Rogue.mTarget.CurrentTarget != Helpers.Rogue.me 
                            && Movement.IsAboveTheGround(Rogue.mTarget) 
                || Rogue.mTarget.IsFriendly || (Rogue.mTarget.Guid == SapCCUnitGUID && Rogue.mTarget.HasAura("Sap")) && mNearbyEnemyUnits.Count() > 1),
                GetNewTarget()
            );
        }

        static private Composite GetNewTarget()
        {
            return new Action(ret =>
                {
                    var nextUnit = mNearbyEnemyUnits.Where(unit => 
                        !(unit.Guid == SapCCUnitGUID && unit.HasAura("Sap") || unit.Guid == BlindCCUnitGUID && unit.HasAura("Blind"))).FirstOrDefault();

                    if (nextUnit != null && nextUnit.IsAlive)
                    {
                        Logging.Write(LogLevel.Normal, "Changing target to " + nextUnit.Name);
                        Helpers.Rogue.mTarget = nextUnit;
                        nextUnit.Target();
                    }
                    else
                    {
                        nextUnit = mNearbyEnemyUnits.FirstOrDefault();


                        if (nextUnit != null && nextUnit.Guid == SapCCUnitGUID && nextUnit.HasAura("Sap") && mNearbyEnemyUnits.Count() == 1)
                        {
                            Logging.Write(LogLevel.Normal, "Clearing Target");
                            Helpers.Rogue.me.ClearTarget();
                            Helpers.Rogue.mTarget = null;
                        }
                        else if (nextUnit != null && nextUnit.IsAlive)
                        {
                            Logging.Write(LogLevel.Normal, "Changing target to " + nextUnit.Name);
                            Helpers.Rogue.mTarget = nextUnit;
                            nextUnit.Target();
                        }
                        else
                        {
                            Logging.Write(LogLevel.Normal, "Clearing Target");
                            Helpers.Rogue.me.ClearTarget();
                            Helpers.Rogue.mTarget = null;
                        }
                    }
                }
            );
        }
    }
}
