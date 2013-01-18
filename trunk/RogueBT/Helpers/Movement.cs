//////////////////////////////////////////////////
//               Movement.cs                    //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Drawing;
using System.Globalization;
using Styx;
using Styx.Helpers;
using Styx.CommonBot;
using Styx.TreeSharp;
using System.Linq;
using Action = Styx.TreeSharp.Action;
using Styx.WoWInternals;
using Styx.Pathing;

namespace RogueBT.Helpers
{
    internal static class Movement
    {
        static Movement()
        {
        }
        static public Composite WalkBackwards()
        {
            return new Sequence(
                new PrioritySelector(
                    new Decorator(ret => Helpers.Rogue.me.MovementInfo.MovingBackward
                        && (!Helpers.Movement.IsInSafeMeleeRange || Helpers.Movement.IsInSafeMeleeRange && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance < Helpers.Rogue.me.CombatReach + 0.3333334f + unit.CombatReach && unit.IsBehind(Helpers.Rogue.me)) == 0),
                        new Action(ret =>
                        {
                            Styx.Common.Logging.Write(Styx.Common.LogLevel.Diagnostic, "walking backwards, stop");
                            WoWMovement.MoveStop(WoWMovement.MovementDirection.Backwards);

                        })),
                    new Decorator(ret => Helpers.Movement.IsInSafeMeleeRange && Helpers.Rogue.mTarget != null && !Helpers.Rogue.mTarget.Stunned && !Helpers.Rogue.mTarget.IsCasting
                        && Navigator.CanNavigateFully(Helpers.Rogue.me.Location, Helpers.Rogue.me.Location.RayCast(Helpers.Rogue.me.Rotation + WoWMathHelper.DegreesToRadians(150), 6f))
                        && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance < Helpers.Rogue.me.CombatReach + 0.3333334f + unit.CombatReach && unit.IsBehind(Helpers.Rogue.me)) > 0,
                        new Action(ret => {

                            Styx.Common.Logging.Write(Styx.Common.LogLevel.Diagnostic, "walking backwards"); 
                            WoWMovement.Move(WoWMovement.MovementDirection.Backwards); 
                        
                        }))
                    ),
                    new Action(ret => RunStatus.Failure)
            );
        }

        public static bool IsGlueEnabled
        {
            get
            {
                int PluginCount = (from MyPlugin in Styx.Plugins.PluginManager.Plugins
                                   where MyPlugin.Name == "Glue" && MyPlugin.Enabled == true
                                   select MyPlugin).Count();

                if (PluginCount >= 1) return true;
                

                return false;
            }
        }

        public static bool IsPVPSuiteEnabled
        {
            get
            {
                int PluginCount = (from MyPlugin in Styx.Plugins.PluginManager.Plugins
                                   where MyPlugin.Name.Contains("Ultimate PVP Suite") && MyPlugin.Enabled == true
                                   select MyPlugin).Count();

                if (PluginCount >= 1)
                    return true;

                return false;
            }
        }

        private static float MeleeRange
        {
            get
            {
                if (Rogue.mTarget == null)
                    return 0f;

                if (Rogue.mTarget.IsPlayer)
                    return 3.5f;

                return System.Math.Max(5f, Helpers.Rogue.me.CombatReach + 1.3333334f + Rogue.mTarget.CombatReach);
            }
        }

        public static float SafeMeleeRange
        {
            get { return System.Math.Max(MeleeRange - 1.9f, 2.5f); }
        }

        public static bool IsInAttemptMeleeRange
        {
            get
            {
                if (Helpers.Rogue.mTarget == null) return false;

                return Helpers.Rogue.mTarget.Distance < System.Math.Max(MeleeRange - 1.2f, 2.5f);
            }
        }

        public static bool IsInSafeMeleeRange
        {
            get
            {
                if (Helpers.Rogue.mTarget == null) return false;
                
                return Helpers.Rogue.mTarget.Distance < System.Math.Max(MeleeRange - 1.4f, 2.5f); 
            }
        }

        private static bool directionChange;

        public static void Pulse()
        {
        }
        public static bool OldStopRunning()
        {
            if (System.Math.Abs(System.Math.Abs(Helpers.Rogue.me.RenderFacing) - System.Math.Abs(Helpers.Aura.LastRenderFacing)) > 1)
            {
                if (!Aura.LastDirection)
                {
                    directionChange = true;
                    Aura.LastDirection = true;
                }
            }
            else
            {
                if (Aura.LastDirection)
                {
                    directionChange = true;
                    Aura.LastDirection = false;
                }
            }
            Helpers.Aura.LastRenderFacing = Helpers.Rogue.me.RenderFacing;
            return true;
        }

        public static bool FNorth { get; private set; }
        public static bool FEast { get; private set; }
        public static bool FWest { get; private set; }
        public static bool FSouth { get; private set; }

        public static bool StopRunning()
        {
            if (Helpers.Rogue.me.RenderFacing < 0.8125 && Helpers.Rogue.me.RenderFacing > -0.8125) 
            {
                FNorth = true;
                if (FSouth)
                {
                    directionChange = true;
                    FNorth = false;
                    FEast = false;
                    FSouth = false;
                    FWest = false;
                }
            }
            else if (Helpers.Rogue.me.RenderFacing > -2.4375 && Helpers.Rogue.me.RenderFacing < -0.8125)
            {

                FWest = true;
                if (FEast)
                {
                    directionChange = true;
                    FNorth = false;
                    FEast = false;
                    FSouth = false;
                    FWest = false;
                }
            }
            else if (Helpers.Rogue.me.RenderFacing < 2.4375 && Helpers.Rogue.me.RenderFacing > 0.8125)
            {

                FEast = true;
                if (FWest)
                {
                    directionChange = true;
                    FNorth = false;
                    FEast = false;
                    FSouth = false;
                    FWest = false;
                }
            }
            else if (Helpers.Rogue.me.RenderFacing > 2.4375 || Helpers.Rogue.me.RenderFacing < -2.4375)
            {

                FSouth = true;
                if (FNorth)
                {
                    directionChange = true;
                    FNorth = false;
                    FEast = false;
                    FSouth = false;
                    FWest = false;
                }
            }
            return true;
        }

        public static Composite PleaseStop()
        {
            return new Decorator(ret => Rogue.mTarget != null && Settings.Mode.mUseMovement && IsInSafeMeleeRange && !Helpers.Rogue.me.MovementInfo.IsStrafing && !Helpers.Rogue.me.MovementInfo.MovingBackward
                 && StopRunning() && directionChange,
                new Action(ret => {
                    Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "please stop");
                directionChange = false;
                Navigator.PlayerMover.MoveStop();
                }));
        }
        public static Composite PleaseStopPull()
        {
            return new Decorator(ret => Rogue.mTarget != null && Settings.Mode.mUseMovement && IsInSafeMeleeRange && !Helpers.Rogue.me.MovementInfo.IsStrafing && !Helpers.Rogue.me.MovementInfo.MovingBackward
                 && StopRunning() && directionChange,
                new Sequence(new Action(ret =>
                {
                    Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "please stop");
                    directionChange = false;
                    Navigator.PlayerMover.MoveStop();
                })
                ,new Action(ret => RunStatus.Failure))
                );
        }

        public static Composite MoveToLos()
        {
            return new Decorator(ret => Rogue.mTarget != null && Settings.Mode.mUseMovement && !Helpers.Rogue.me.MovementInfo.IsStrafing
                && !((!Rogue.mTarget.IsPlayer && !IsInSafeMeleeRange) || Rogue.mTarget.IsPlayer && Helpers.Rogue.mTarget.Distance > 2.5f) &&//!IsInSafeMeleeRange && 
            !Rogue.mTarget.InLineOfSight,
                new Action(ret => Navigator.MoveTo(Rogue.mTarget.Location)));
        }

        public static Composite ChkFace()
        { 
            return new Decorator(ret => Rogue.mTarget != null && Settings.Mode.mUseMovement && !Helpers.Rogue.me.MovementInfo.IsStrafing
                && ((!Rogue.mTarget.IsPlayer && IsInSafeMeleeRange) || Rogue.mTarget.IsPlayer && Helpers.Rogue.mTarget.Distance < 10) && !Helpers.Rogue.me.IsSafelyFacing(Rogue.mTarget),
                                          new Sequence(new Action(ret =>
                                          {
                                              if (Helpers.Rogue.me.IsActuallyInCombat || Rogue.mTarget.IsPlayer)
                                              Navigator.PlayerMover.MoveStop();
                                              Styx.Common.Logging.Write(Styx.Common.LogLevel.Diagnostic, "facing");
                                              Rogue.mTarget.Face();
                                          }), new Action(ret => RunStatus.Failure)));
        }
        public static Composite PullMoveToTarget()
        {
            return new PrioritySelector(
             new Decorator(ret => Helpers.Rogue.me != null && Rogue.mTarget != null
                 && Helpers.Rogue.me.IsMoving
                 && Styx.CommonBot.POI.BotPoi.Current != null
                 && Styx.CommonBot.POI.BotPoi.Current.Type != Styx.CommonBot.POI.PoiType.Kill 
                 && !Helpers.Rogue.me.IsSafelyFacing(Rogue.mTarget)
                 && !(Helpers.Area.mLocation.Equals(Enum.LocationContext.Battleground))
                 || Helpers.Aura.Stealth && Helpers.Rogue.mHP < 20
                 ,
                 new Action(ret => Helpers.Rogue.me.ClearTarget())),
             new Decorator(ret => Rogue.mTarget != null,
                 MoveToTarget()));
        }
        public static Composite MoveToTarget()
        {
            return new Decorator(
                ret => !Helpers.Rogue.me.IsCasting && Rogue.mTarget != null && Settings.Mode.mUseMovement && !Helpers.Rogue.me.Mounted
                    && !Rogue.mTarget.IsFriendly && !Helpers.Rogue.me.MovementInfo.IsStrafing
                    && !(Rogue.mTarget.Distance < 10 && IsGlueEnabled) 
                    //!Aura.HealingGhost && Rogue.mTarget.Attackable && Rogue.mTarget.IsHostile &&  
                   //|| Helpers.Rogue.me.Stunned || Helpers.Rogue.me.Rooted || Aura.IsTargetSapped && Rogue.mTarget.IsAlive|| Aura.IsTargetDisoriented
                //&& !Helpers.Aura.IsTargetInvulnerable 
                ,
                new PrioritySelector(
                    new Decorator(ret => !Settings.Mode.mMoveBehind || !Rogue.mTarget.IsPlayer && Rogue.mTarget.CurrentTarget == Helpers.Rogue.me,
                                  new Sequence(
                                      new DecoratorContinue(
                                          ret => !IsInSafeMeleeRange,
                                          new Action(ret => Navigator.MoveTo(Rogue.mTarget.Location))),
                                      new DecoratorContinue(
                                          ret =>
                                          IsInSafeMeleeRange &&
                                          !Helpers.Rogue.me.IsSafelyFacing(Rogue.mTarget),
                                          new Action(ret => Rogue.mTarget.Face()))
                                      ,new Action(ret => RunStatus.Failure)
                                      )
                        ),
                    new Decorator(ret => Rogue.mTarget.IsPlayer || Rogue.mTarget.CurrentTarget != Helpers.Rogue.me,
                                  new Sequence(
                                      new DecoratorContinue(
                                          ret => (!Helpers.Rogue.me.IsMoving || Rogue.mTarget.IsMoving)
                                         && (!Aura.IsBehind || Helpers.Rogue.mTarget.Distance > SafeMeleeRange ),
                                          new PrioritySelector(
                                              new Decorator(
                                                  ret => Navigator.CanNavigateFully(Helpers.Rogue.me.Location, Rogue.mTarget.Location.RayCast(Rogue.mTarget.Rotation + WoWMathHelper.DegreesToRadians(150), SafeMeleeRange))
                                                     && System.Math.Abs(HeightOffTheGround(Rogue.mTarget.Location.RayCast(Rogue.mTarget.Rotation + WoWMathHelper.DegreesToRadians(150), SafeMeleeRange))) < 3,
                                                new Action(ret => Navigator.MoveTo(Rogue.mTarget.Location.RayCast(Rogue.mTarget.Rotation + WoWMathHelper.DegreesToRadians(150), SafeMeleeRange)))),
                                              new Decorator(
                                                  ret => true,
                                                  new Action(ret => Navigator.MoveTo(Rogue.mTarget.Location))))
                                     ),
                                      new DecoratorContinue(
                                          ret => IsInSafeMeleeRange &&
                                          !Helpers.Rogue.me.IsSafelyFacing(Rogue.mTarget),
                                          new Action(ret => Rogue.mTarget.Face())),
                                      new Action(ret => RunStatus.Failure)
                                      )
                 )));
        }


        /// stolen from singular
        /// <summary>
        /// determines if a target is off the ground far enough that you can
        /// reach it with melee spells if standing directly under.
        /// </summary>
        /// <param name="u">unit</param>
        /// <returns>true if above melee reach</returns>
        public static bool IsAboveTheGround(this Styx.WoWInternals.WoWObjects.WoWUnit u)
        {
            float height = HeightOffTheGround(u);
            if (height == float.MaxValue)
                return false;   // make this true if better to assume aerial 

            if (height > System.Math.Max(Helpers.Rogue.me.CombatReach - 0.1f + u.CombatReach, 2.5f))
                return true;

            return false;
        }

        /// stolen from singular
        /// <summary>
        /// calculate a unit's vertical distance (height) above ground level (mesh).  this is the units position
        /// relative to the ground and is independent of any other character.  
        /// </summary>
        /// <param name="u">unit</param>
        /// <returns>float.MinValue if can't determine, otherwise distance off ground</returns>
        public static float HeightOffTheGround(this Styx.WoWInternals.WoWObjects.WoWUnit u)
        {
            var unitLoc = new WoWPoint(u.Location.X, u.Location.Y, u.Location.Z);
            var listMeshZ = Navigator.FindHeights(unitLoc.X, unitLoc.Y).Where(h => h <= unitLoc.Z);
            if (listMeshZ.Any())
                return unitLoc.Z - listMeshZ.Max();

            return float.MaxValue;
        }
        /// stolen from singular
        /// <summary>
        /// calculate a point's vertical distance (height) above ground level (mesh).  this is the units position
        /// relative to the ground and is independent of any other character.  
        /// </summary>
        /// <param name="u">unit</param>
        /// <returns>float.MinValue if can't determine, otherwise distance off ground</returns>
        public static float HeightOffTheGround(this WoWPoint unitLoc)
        {
            var listMeshZ = Navigator.FindHeights(unitLoc.X, unitLoc.Y).Where(h => h <= unitLoc.Z);
            if (listMeshZ.Any())
                return unitLoc.Z - listMeshZ.Max();

            return float.MaxValue;
        }
    }
}