//////////////////////////////////////////////////
//               Movement.cs                    //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Drawing;
using CommonBehaviors.Actions;
using Styx;
using Styx.Helpers;
using Styx.Logic.Pathing;
using TreeSharp;
using System.Linq;

namespace RogueRaidBT.Helpers
{
    static class Movement
    {
        static bool directionChange;
        static public void Pulse()
        {
            Rogue.mTarget = StyxWoW.Me.CurrentTarget;
            if (Helpers.Rogue.mTarget != null && Settings.Mode.mUseMovement && !Rogue.mTarget.IsFriendly && Rogue.mTarget.IsAlive &&
                !Aura.HealingGhost &&
                !Helpers.Aura.IsTargetDisoriented && !Helpers.Aura.IsTargetSapped && !Helpers.Aura.IsTargetInvulnerable //&& (!Rogue.mTarget.IsPlayer || Rogue.mTarget.PvpFlagged)
                ) MoveBehind();
            

        }

        static public Composite MoveToLoc(WoWPointDelegate loc)
        {
            return MoveToLoc(loc, 2);
        }

        static public Composite MoveToLoc(WoWPointDelegate loc, float distance)
        {
            return new PrioritySelector(
                new Decorator(ret => loc(ret).Distance(StyxWoW.Me.Location) > distance,
                    new Action(ret => Navigator.MoveTo(loc(ret)))),

                new Decorator(ret => loc(ret).Distance(StyxWoW.Me.Location) <= distance - 1 &&
                                     StyxWoW.Me.IsMoving,
                    new Action(ret => Navigator.PlayerMover.MoveStop()))
            );
        }

        static public Composite MoveToUnit(WoWUnitDelegate unit)
        {
            return MoveToLoc(ret => unit(ret).Location, 5);
        }

        static public Composite FaceUnit(WoWUnitDelegate unit)
        {
            return new Decorator(ret => !StyxWoW.Me.IsSafelyFacing(unit(ret)),
                new Action(ret => unit(ret).Face())
            );
        }

        //Glue Friendly!!!
        static public void MoveBehind()
        {
            if (Rogue.mTarget.Distance < 10 && IsGlueEnabled ||
                StyxWoW.Me.Stunned || StyxWoW.Me.Rooted)
                return;
            
            if (StyxWoW.Me.MovementInfo.Heading - Rogue.mTarget.MovementInfo.Heading>0)
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
                    //Logging.Write(Color.White, "change");
            
                    directionChange = true;
                    Aura.LastDirection = false;
                }
            }

            if (!StyxWoW.Me.Mounted && (!Aura.IsBehind || !Rogue.mTarget.IsWithinMeleeRange))
                Navigator.MoveTo(Styx.Helpers.WoWMathHelper.CalculatePointBehind(
                    Helpers.Rogue.mTarget.Location, Helpers.Rogue.mTarget.Rotation, 1.0f));

            if (Rogue.mTarget.IsWithinMeleeRange && Aura.IsBehind && directionChange)
            {
                //Logging.Write(Color.White, "stopping");
            
                directionChange = false;
                Styx.WoWInternals.WoWMovement.MoveStop();

            }
                
            if (Rogue.mTarget.Distance > 0.6 && Rogue.mTarget.Distance < 6 &&
                !StyxWoW.Me.IsSafelyFacing(Rogue.mTarget))
            {

                Rogue.mTarget.Face();
            }


            
            

            
          
        }


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

        static public Composite MoveToAndFaceUnit(WoWUnitDelegate unit)
        {
            return new Decorator(ret => Settings.Mode.mUseMovement && StyxWoW.Me.CurrentTarget != null &&
                            !StyxWoW.Me.CurrentTarget.IsWithinMeleeRange, 
                new PrioritySelector(
                    MoveToUnit(unit),
                    FaceUnit(unit)
                )
            );
        }
    }
}
