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

        static public Composite WalkBackwards()
        {
            return new Decorator(ret => true, //(!Helpers.Target.mNearbyEnemyUnits.Contains(Rogue.mTarget) || Rogue.mTarget == null) && !BotManager.Current.Name.Equals("BGBuddy"),
                new Action(ret => {WoWMovement.Move(WoWMovement.MovementDirection.Backwards);})
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
            get { return System.Math.Max(MeleeRange - 1f, 4.7f); }
        }

        public static bool IsInSafeMeleeRange
        {
            get
            {
                if (Helpers.Rogue.mTarget == null) return false;
                
                return Helpers.Rogue.mTarget.Distance < System.Math.Max(MeleeRange - 1f, 2.5f); 
            }
        }

        private static bool directionChange;

        public static void Pulse()
        {
        }

        public static bool StopRunning()
        {
            if (System.Math.Abs(System.Math.Abs(Helpers.Rogue.me.RenderFacing) - System.Math.Abs(Helpers.Aura.LastRenderFacing)) > 1) //|| Helpers.Rogue.me.MovementInfo.Heading.CompareTo(Rogue.mTarget.MovementInfo.Heading) > 0 && Helpers.Aura.IsBehind
            {
                if (!Aura.LastDirection)
                {
                    //Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "" + System.Math.Abs(System.Math.Abs(Helpers.Rogue.me.RenderFacing) - System.Math.Abs(Helpers.Aura.LastRenderFacing)));
            
                    directionChange = true;
                    Aura.LastDirection = true;
                }
            }
            else
            {
                if (Aura.LastDirection)
                {
                    //Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "" + System.Math.Abs(System.Math.Abs(Helpers.Rogue.me.RenderFacing) - System.Math.Abs(Helpers.Aura.LastRenderFacing)));
            
                    directionChange = true;
                    Aura.LastDirection = false;
                }
            }

            Helpers.Aura.LastRenderFacing = Helpers.Rogue.me.RenderFacing;

            
            return true;
        }

        public static Composite PleaseStop()
        {
            return new Decorator(ret => Rogue.mTarget != null && Settings.Mode.mUseMovement && IsInSafeMeleeRange && !Helpers.Rogue.me.MovementInfo.IsStrafing &&
                 StopRunning() && directionChange,
                new Action(ret => {
                    Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "please stop");
                directionChange = false;
                Navigator.PlayerMover.MoveStop();
                //if (Rogue.mTarget.Distance < 2d && Rogue.mTarget.CurrentTarget != Helpers.Rogue.me)
                   // Navigator.MoveTo(Rogue.mTarget.Location.RayCast( Rogue.mTarget.Rotation + WoWMathHelper.DegreesToRadians(150),  MeleeRange - 1f));
                
                }));
        }
        public static Composite PleaseStopPull()
        {
            return new Decorator(ret => Rogue.mTarget != null && Settings.Mode.mUseMovement && IsInSafeMeleeRange && !Helpers.Rogue.me.MovementInfo.IsStrafing &&
                 StopRunning() && directionChange,
                new Action(ret =>
                {
                    Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "please stop");
                    directionChange = false;
                    Navigator.PlayerMover.MoveStop();
                    Navigator.MoveTo(Rogue.mTarget.Location.RayCast(Rogue.mTarget.Rotation + WoWMathHelper.DegreesToRadians(150), MeleeRange - 1f));

                }));
        }

        public static Composite MoveToLos()
        {
            return new Decorator(ret => Rogue.mTarget != null && Settings.Mode.mUseMovement && !((!Rogue.mTarget.IsPlayer && !IsInSafeMeleeRange) || Rogue.mTarget.IsPlayer && Helpers.Rogue.mTarget.Distance > 2.5f) &&//!IsInSafeMeleeRange && 
            !Rogue.mTarget.InLineOfSight,
                new Action(ret => Navigator.MoveTo(Rogue.mTarget.Location)));
        }

        public static Composite ChkFace()
        { //Rogue.mTarget.Distance > 0.6 && Rogue.mTarget.Distance < 6 &&
            return new Decorator(ret => Rogue.mTarget != null && ((!Rogue.mTarget.IsPlayer && IsInSafeMeleeRange) || Rogue.mTarget.IsPlayer && Helpers.Rogue.mTarget.Distance < 10) && !Helpers.Rogue.me.IsSafelyFacing(Rogue.mTarget),
                                          new Action(ret =>
                                          {
                                              if (Helpers.Rogue.me.IsActuallyInCombat || Rogue.mTarget.IsPlayer)
                                              Navigator.PlayerMover.MoveStop();
                                              Styx.Common.Logging.Write(Styx.Common.LogLevel.Diagnostic, "facing");
                                              Rogue.mTarget.Face();
                                          }));
        }
        public static Composite PullMoveToTarget()
        {
            //Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "pull");
            return new PrioritySelector(
             new Decorator(ret => (Helpers.Rogue.me.IsMoving && Styx.CommonBot.POI.BotPoi.Current.Type != Styx.CommonBot.POI.PoiType.Kill && !Helpers.Rogue.me.IsSafelyFacing(Rogue.mTarget)
                 || !Rogue.mTarget.Attackable 
//  || (Styx.CommonBot.POI.BotPoi.Current.Type != Styx.CommonBot.POI.PoiType.QuestTurnIn || Styx.CommonBot.POI.BotPoi.Current.Type != Styx.CommonBot.POI.PoiType.QuestPickUp) && Rogue.mHP >50
                ) && Rogue.mTarget != null && !BotManager.Current.Name.Equals("BGBuddy")
                 //|| Rogue.mTarget.CreatedByUnitGuid != Helpers.Rogue.me.Guid
                 ,
                 new Action(ret => Helpers.Rogue.me.ClearTarget())),
             new Decorator(ret => true, MoveToTarget()));
                //!Helpers.Rogue.me.IsCasting && Helpers.Rogue.me.CurrentTarget != null && Helpers.Rogue.me.CurrentTarget.IsWithinMeleeRange, 
                //new Action(ret => Navigator.MoveTo(Helpers.Rogue.me.CurrentTarget.Location))));
        }
        public static Composite MoveToTarget()
        {
            //if (StyxWoW.Me.IsActuallyInCombat && Movement.MoveTo(StyxWoW.Me.CurrentTarget)) { Blacklist.Flush(); }
            //change dec continue
            return new Decorator(
                ret => Rogue.mTarget != null && Settings.Mode.mUseMovement && !Helpers.Rogue.me.Mounted &&
                    //!Aura.HealingGhost && Rogue.mTarget.Attackable && Rogue.mTarget.IsHostile &&  
                                        !(Rogue.mTarget.Distance < 10 && IsGlueEnabled) //|| Helpers.Rogue.me.Stunned || Helpers.Rogue.me.Rooted || Aura.IsTargetSapped && Rogue.mTarget.IsAlive|| Aura.IsTargetDisoriented
                //&& !Helpers.Aura.IsTargetInvulnerable 
                ,
                new PrioritySelector(
                    new Decorator(ret => Rogue.mTarget.CurrentTarget != Helpers.Rogue.me || Rogue.mTarget.IsPlayer,//&& Helpers.Rogue.me.GroupInfo.IsInParty
                                  new Sequence(

                                      new DecoratorContinue(
                                          ret =>
                                          (!Aura.IsBehind || ((!Rogue.mTarget.IsPlayer && !IsInSafeMeleeRange) || Rogue.mTarget.IsPlayer && Helpers.Rogue.mTarget.Distance > 2.5f) ||
                                                Rogue.mTarget.IsMoving) && !Helpers.Rogue.me.IsCasting &&
                                          (!Helpers.Rogue.me.IsMoving || Rogue.mTarget.IsMoving), // (!Aura.IsBehind || Rogue.mTarget.Distance > MeleeRange - 3f) && !Helpers.Rogue.me.IsCasting,
                                          new Action(ret => Navigator.MoveTo(Rogue.mTarget.Location.RayCast(
                                                                 Rogue.mTarget.Rotation +
                                                                 WoWMathHelper.DegreesToRadians(150),
                                                                 MeleeRange - 1f)))),
                                      new DecoratorContinue(
                                          ret => IsInSafeMeleeRange &&
                                              //Rogue.mTarget.Distance > 0.6 && Rogue.mTarget.Distance < 6 &&
                                          !Helpers.Rogue.me.IsSafelyFacing(Rogue.mTarget),
                                          new Action(ret => Rogue.mTarget.Face())),
                                      new Action(ret => RunStatus.Failure)
                                      )
                        ),
                    new Decorator(ret => !Rogue.mTarget.IsPlayer,
                                  new Sequence(

                                      new DecoratorContinue(
                                          ret => !IsInSafeMeleeRange && !Helpers.Rogue.me.IsCasting,
                                          new Action(ret =>
                                          {
                                              //Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "move " + IsInSafeMeleeRange);
                                              Navigator.MoveTo(Rogue.mTarget.Location);
                                          })),

                                      new DecoratorContinue(
                                          ret =>
                                          IsInSafeMeleeRange &&
                                          !Helpers.Rogue.me.IsSafelyFacing(Rogue.mTarget),
                                          new Action(ret => Rogue.mTarget.Face())),
                                      new Action(ret => RunStatus.Failure)
                                      )
                        ))
                );
        }
    }
}