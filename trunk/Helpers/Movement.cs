//////////////////////////////////////////////////
//               Movement.cs                    //
//      Part of RogueRaidBT by kaihaider        //
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

namespace RogueRaidBT.Helpers
{
    internal static class Movement
    {
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

                return System.Math.Max(5f, StyxWoW.Me.CombatReach + 1.3333334f + Rogue.mTarget.CombatReach);
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
            if (System.Math.Abs(System.Math.Abs(StyxWoW.Me.RenderFacing) - System.Math.Abs(Helpers.Aura.LastRenderFacing)) > 1) //|| StyxWoW.Me.MovementInfo.Heading.CompareTo(Rogue.mTarget.MovementInfo.Heading) > 0 && Helpers.Aura.IsBehind
            {
                if (!Aura.LastDirection)
                {
                    //Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "" + System.Math.Abs(System.Math.Abs(StyxWoW.Me.RenderFacing) - System.Math.Abs(Helpers.Aura.LastRenderFacing)));
            
                    directionChange = true;
                    Aura.LastDirection = true;
                }
            }
            else
            {
                if (Aura.LastDirection)
                {
                    //Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "" + System.Math.Abs(System.Math.Abs(StyxWoW.Me.RenderFacing) - System.Math.Abs(Helpers.Aura.LastRenderFacing)));
            
                    directionChange = true;
                    Aura.LastDirection = false;
                }
            }

            Helpers.Aura.LastRenderFacing = StyxWoW.Me.RenderFacing;

            
            return true;
        }

        public static Composite PleaseStop()
        {
            return new Decorator(ret => Rogue.mTarget != null && Settings.Mode.mUseMovement && IsInSafeMeleeRange && !StyxWoW.Me.MovementInfo.IsStrafing &&
                 StopRunning() && directionChange,
                new Action(ret => {
                    Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "please stop");
                directionChange = false;
                Navigator.PlayerMover.MoveStop();
                //if (Rogue.mTarget.Distance < 2d && Rogue.mTarget.CurrentTarget != StyxWoW.Me)
                   // Navigator.MoveTo(Rogue.mTarget.Location.RayCast( Rogue.mTarget.Rotation + WoWMathHelper.DegreesToRadians(150),  MeleeRange - 1f));
                
                }));
        }
        public static Composite PleaseStopPull()
        {
            return new Decorator(ret => Rogue.mTarget != null && Settings.Mode.mUseMovement && IsInSafeMeleeRange && !StyxWoW.Me.MovementInfo.IsStrafing &&
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
        {
            return new Decorator(ret => Rogue.mTarget != null && ((!Rogue.mTarget.IsPlayer && IsInSafeMeleeRange) || Rogue.mTarget.IsPlayer && Helpers.Rogue.mTarget.IsWithinMeleeRange) && !StyxWoW.Me.IsSafelyFacing(Rogue.mTarget),
                                          new Action(ret =>
                                          { if(StyxWoW.Me.IsActuallyInCombat)
                                              Navigator.PlayerMover.MoveStop();
                                              Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "facing");
                                              Rogue.mTarget.Face();
                                          }));
        }
        public static Composite PullMoveToTarget()
        {
           return  new PrioritySelector(
            new Decorator(ret => StyxWoW.Me.IsMoving && Rogue.mTarget != null && !StyxWoW.Me.IsSafelyFacing(Rogue.mTarget) && !BotManager.Current.Name.Equals("BGBuddy"), 
                new Action(ret => StyxWoW.Me.ClearTarget())),
            new Decorator(ret => true, new Action(ret =>  MoveToTarget())));
        }
        public static Composite MoveToTarget()
        {
            
            //change dec continue
            return new Decorator(
                ret => Rogue.mTarget != null && Settings.Mode.mUseMovement &&
                                        //!Aura.HealingGhost && Rogue.mTarget.Attackable && Rogue.mTarget.IsHostile && 
                                        !(Rogue.mTarget.Distance < 10 && IsGlueEnabled || StyxWoW.Me.Stunned ||
                                        StyxWoW.Me.Rooted) // || Aura.IsTargetSapped && Rogue.mTarget.IsAlive|| Aura.IsTargetDisoriented
                //&& !Helpers.Aura.IsTargetInvulnerable 
                ,
                new PrioritySelector(
                    new Decorator(ret => Rogue.mTarget.CurrentTarget != StyxWoW.Me || Rogue.mTarget.IsPlayer,//&& StyxWoW.Me.GroupInfo.IsInParty
                                  new Sequence(

                                      new DecoratorContinue(
                                          ret =>
                                          !StyxWoW.Me.Mounted &&
                                          (!Aura.IsBehind || ((!Rogue.mTarget.IsPlayer && !IsInSafeMeleeRange) || Rogue.mTarget.IsPlayer && Helpers.Rogue.mTarget.Distance > 2.5f) ||  
                                                Rogue.mTarget.IsMoving) && !StyxWoW.Me.IsCasting && 
                                          (!StyxWoW.Me.IsMoving || Rogue.mTarget.IsMoving), // (!Aura.IsBehind || Rogue.mTarget.Distance > MeleeRange - 3f) && !StyxWoW.Me.IsCasting,
                                          new Action(ret => Navigator.MoveTo(Rogue.mTarget.Location.RayCast(
                                                                 Rogue.mTarget.Rotation +
                                                                 WoWMathHelper.DegreesToRadians(150),
                                                                 MeleeRange - 1f)))),
                                      //new DecoratorContinue(
                                      //    ret =>
                                      //    Aura.IsBehind && IsInSafeMeleeRange && !Rogue.mTarget.MovementInfo.IsMoving && // Rogue.mTarget.Distance < MeleeRange - 3f &&
                                      //    StyxWoW.Me.MovementInfo.MovingForward,
                                      //    new Action(ret =>
                                      //                   {
                                      //                       Styx.Common.Logging.Write(Styx.Common.LogLevel.Normal, "please stop!"); 
                                      //                       directionChange = false;
                                      //                       Navigator.PlayerMover.MoveStop();
                                      //                   })),
                                      new DecoratorContinue(
                                          ret => IsInSafeMeleeRange &&
                                          //Rogue.mTarget.Distance > 0.6 && Rogue.mTarget.Distance < 6 &&
                                          !StyxWoW.Me.IsSafelyFacing(Rogue.mTarget),
                                          new Action(ret => Rogue.mTarget.Face())),
                                      new Action(ret => RunStatus.Failure)
                                      )
                        ),
                    new Decorator(ret => !Rogue.mTarget.IsPlayer, 
                                  new Sequence(

                                      new DecoratorContinue(
                                          ret => !StyxWoW.Me.Mounted && !IsInSafeMeleeRange && !StyxWoW.Me.IsCasting,
                                          new Action(ret => Navigator.MoveTo(Rogue.mTarget.Location))),
                                      
                                      new DecoratorContinue(
                                          ret =>
                                          IsInSafeMeleeRange &&
                                          !StyxWoW.Me.IsSafelyFacing(Rogue.mTarget),
                                          new Action(ret => Rogue.mTarget.Face())),
                                      new Action(ret => RunStatus.Failure)
                                      )
                        ))
                );
        }
    }
}