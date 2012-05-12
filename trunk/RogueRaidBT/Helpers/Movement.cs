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
using Styx.Logic.Pathing;
using TreeSharp;
using System.Linq;
using Action = TreeSharp.Action;
using Styx.WoWInternals;

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
            get { return System.Math.Max(MeleeRange - 1f, 5f); }
        }

        private static bool directionChange;

        public static void Pulse()
        {
        }

        public static bool StopRunning()
        {
            if (StyxWoW.Me.MovementInfo.Heading - Rogue.mTarget.MovementInfo.Heading > 0)
            {
                if (!Aura.LastDirection)
                {
                    //Logging.Write(Color.White, "change");
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

            return true;
        }

        public static Composite MoveToTarget()
        {
            //change dec continue
            return new Decorator(
                ret => StopRunning()
                /* && Rogue.mTarget != null && Settings.Mode.mUseMovement && Rogue.mTarget.IsAlive &&
                                        !Aura.HealingGhost && Rogue.mTarget.Attackable && Rogue.mTarget.IsHostile &&
                                        !Rogue.mTarget.IsFriendly && !Aura.IsTargetDisoriented && !Aura.IsTargetSapped &&
                                        
                                        !(Rogue.mTarget.Distance < 10 && IsGlueEnabled || StyxWoW.Me.Stunned || StyxWoW.Me.Rooted)
                                 //&& !Helpers.Aura.IsTargetInvulnerable //&& (!Rogue.mTarget.IsPlayer || Rogue.mTarget.PvpFlagged)**/
                ,
                new PrioritySelector(
                    new Decorator(ret => Rogue.mTarget.CurrentTarget != StyxWoW.Me || Rogue.mTarget.IsPlayer,
                                  new Sequence(
                                      new DecoratorContinue(
                                          ret =>
                                          !StyxWoW.Me.Mounted &&
                                          (!Aura.IsBehind || Rogue.mTarget.Distance > MeleeRange - 2f) && !StyxWoW.Me.IsCasting,
                                          new Action(ret => Navigator.MoveTo(Rogue.mTarget.Location.RayCast(
                                                                 Rogue.mTarget.Rotation +
                                                                 WoWMathHelper.DegreesToRadians(150),
                                                                 MeleeRange - 2f)))),
                                      new DecoratorContinue(
                                          ret =>
                                          Aura.IsBehind && directionChange && Rogue.mTarget.Distance < MeleeRange - 2f &&
                                          StyxWoW.Me.MovementInfo.MovingForward,
                                          new Action(ret =>
                                                         {
                                                             directionChange = false;
                                                             Navigator.PlayerMover.MoveStop();
                                                         })),
                                      new DecoratorContinue(
                                          ret =>
                                          Rogue.mTarget.Distance > 0.6 && Rogue.mTarget.Distance < 6 &&
                                          !StyxWoW.Me.IsSafelyFacing(Rogue.mTarget),
                                          new Action(ret => Rogue.mTarget.Face())),
                                      new Action(ret => RunStatus.Failure)
                                      )
                        ),
                    new Decorator(ret => Rogue.mTarget.CurrentTarget == StyxWoW.Me && !Rogue.mTarget.IsPlayer, 
                                  new Sequence(
                                      new DecoratorContinue(
                                          ret => !StyxWoW.Me.Mounted && Rogue.mTarget.Distance > MeleeRange - 2f && !StyxWoW.Me.IsCasting,
                                          new Action(ret => Navigator.MoveTo(Rogue.mTarget.Location))),
                                      new DecoratorContinue(ret => directionChange && Rogue.mTarget.Distance < MeleeRange &&
                                          StyxWoW.Me.MovementInfo.MovingForward,
                                                            new Action(ret =>
                                                            {
                                                                directionChange = false;
                                                                Navigator.PlayerMover.MoveStop();
                                                            })),
                                      new DecoratorContinue(
                                          ret =>
                                          Rogue.mTarget.Distance > 0.6 && Rogue.mTarget.Distance < 6 &&
                                          !StyxWoW.Me.IsSafelyFacing(Rogue.mTarget),
                                          new Action(ret => Rogue.mTarget.Face())),
                                      new Action(ret => RunStatus.Failure)
                                      )
                        ))
                );
        }


        //Glue Friendly!!!
        public static void MoveBehind()
        {
            if (Rogue.mTarget.Distance < 10 && IsGlueEnabled ||
                StyxWoW.Me.Stunned || StyxWoW.Me.Rooted)
                return;


            if (StyxWoW.Me.MovementInfo.Heading - Rogue.mTarget.MovementInfo.Heading > 0)
            {
                if (!Aura.LastDirection)
                {
                    //Logging.Write(Color.White, "change");
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


            if (!StyxWoW.Me.Mounted &&
                (!Aura.IsBehind || Rogue.mTarget.IsPlayer && Rogue.mTarget.Distance > MeleeRange - 2f))
            {
                Logging.Write(Color.White, Rogue.mTarget.Distance.ToString(CultureInfo.InvariantCulture));
                Navigator.MoveTo(Rogue.mTarget.Location.RayCast(
                    Rogue.mTarget.Rotation + WoWMathHelper.DegreesToRadians(150), MeleeRange - 2f));
            }

            //dec continue
            if (Aura.IsBehind && directionChange && Rogue.mTarget.Distance < MeleeRange - 2f)
            {
                directionChange = false;
                WoWMovement.MoveStop();
            }

            //dec continue
            if (Rogue.mTarget.Distance > 0.6 && Rogue.mTarget.Distance < 6 &&
                !StyxWoW.Me.IsSafelyFacing(Rogue.mTarget))
            {
                Rogue.mTarget.Face();
            }
        }
    }
}